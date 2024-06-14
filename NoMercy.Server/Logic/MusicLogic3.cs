using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using FFMpegCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.AcoustId.Client;
using NoMercy.Providers.AcoustId.Models;
using NoMercy.Providers.MusicBrainz.Client;
using NoMercy.Providers.MusicBrainz.Models;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public partial class MusicLogic3 : IAsyncDisposable
{
    private AcoustIdFingerprint? FingerPrint { get; set; }
    private ConcurrentBag<MediaFile>? Files { get; set; }
    private MediaFolder ListPath { get; set; }
    
    private int Year { get; set; }
    private string AlbumName { get; set; }
    private string ArtistName { get; set; }
    private Library Library { get; set; }
    private Folder? Folder { get; set; }
    
    public MusicLogic3(Library library, MediaFolder listPath)
    {
        Library = library;
        ListPath = listPath;
        
        Files = listPath.Files;
        
        var match = PathRegex().Match(listPath.Path);

        ArtistName = match.Groups["artist"].Success ? match.Groups["artist"].Value : string.Empty;
        AlbumName = match.Groups["album"].Success ? match.Groups["album"].Value : string.Empty;
        Year = match.Groups["year"].Success ? Convert.ToInt32(match.Groups["year"].Value) : 1970;
        
        var libraryFolder = (match.Groups["library_folder"].Success ? match.Groups["library_folder"].Value : null) ?? string.Empty;
        
        Folder = Library.FolderLibraries
            .Select(folderLibrary => folderLibrary.Folder)
            .FirstOrDefault(folder => folder.Path == libraryFolder);
    }

    public async Task Process()
    { 
        await Parallel.ForEachAsync(Files ?? [], async (file, t) =>
        {
            try
            {
                var mediaAnalysis = await FFProbe.AnalyseAsync(file.Path, cancellationToken: t);
                
                var fingerPrintRecording = await MatchTrack(file, mediaAnalysis);

                await Parallel.ForEachAsync(fingerPrintRecording?.Releases ?? [], t, async (release, _) =>
                {
                    if (release.TrackCount == null || release.TrackCount != Files?.Count) return;

                    try
                    {
                        await ProcessRelease(release, file);
                    }
                    catch (Exception e)
                    {
                        if(e.Message.Contains("404")) return;
                        Logger.MusicBrainz(e.Message, LogLevel.Error);
                    }
                });
            }
            catch (Exception e)
            {
                if(e.Message.Contains("404")) return;
                Logger.MusicBrainz(e.Message, LogLevel.Error);
            }
        });
    }

    private async Task ProcessRelease(AcoustIdFingerprintReleaseGroups release, MediaFile mediaFile)
    {
        MusicBrainzReleaseClient musicBrainzReleaseClient = new(release.Id);
        var releaseAppends = await musicBrainzReleaseClient.WithAllAppends();

        if (releaseAppends is null) return;
        
        if (await StoreReleaseGroups(releaseAppends) is null) return;

        if (await StoreRelease(releaseAppends, mediaFile) is null) return;
        await LinkReleaseToReleaseGroup(releaseAppends);
        await LinkReleaseToLibrary(releaseAppends);
        
        foreach (var media in releaseAppends.Media)
        {
            foreach (var track in media.Tracks)
            {
                if (await StoreTrack(releaseAppends, track, media, mediaFile) is null) continue;
                
                await LinkTrackToRelease(track, releaseAppends);

                foreach (var artist in track.ArtistCredit)
                {
                    await StoreArtist(artist.MusicBrainzArtist);
                    await LinkArtistToTrack(artist.MusicBrainzArtist, track);

                    await LinkArtistToAlbum(artist.MusicBrainzArtist, releaseAppends);
                    await LinkArtistToLibrary(artist.MusicBrainzArtist);
                    
                    await LinkArtistToReleaseGroup(releaseAppends, artist.MusicBrainzArtist.Id);
                }
            }
        }
        
        await Task.CompletedTask;
    }

    private async Task<AcoustIdFingerprintRecording?> MatchTrack(MediaFile file, IMediaAnalysis mediaAnalysis)
    {
        using AcoustIdFingerprintClient acoustIdFingerprintClient = new();
        FingerPrint = await acoustIdFingerprintClient.Lookup(file.Path);
        if(FingerPrint is null) return null;

        AcoustIdFingerprintRecording? fingerPrintRecording = null;
        
        foreach (var fingerPrint in FingerPrint?.Results ?? [])
        {
            foreach (var recording in fingerPrint.Recordings ?? [])
            {
                fingerPrintRecording = MatchRelease(file, recording, mediaAnalysis);
                
                if (fingerPrintRecording is not null) break;
                
                fingerPrintRecording = MatchRelease(file, recording, mediaAnalysis, false);
            }
        }
        
        return fingerPrintRecording;
    }

    private AcoustIdFingerprintRecording? MatchRelease(MediaFile file, AcoustIdFingerprintRecording? recording, IMediaAnalysis mediaAnalysis, bool strictMatch = true)
    {
        if (recording is null) return null;
        
        AcoustIdFingerprintRecording? fingerPrintRecording = null;
        
        foreach (var release in  recording.Releases ?? [])
        {
            var matchesTrackCount = release.TrackCount != null && release.TrackCount == Files?.Count;
            if(!matchesTrackCount) continue;

            var fileNameSanitized = file.Parsed?.Title?.RemoveDiacritics().RemoveNonAlphaNumericCharacters() ?? string.Empty;
            var recordNameSanitized = recording.Title?.RemoveDiacritics().RemoveNonAlphaNumericCharacters() ?? string.Empty;
            var matchesName = !fileNameSanitized.Equals(string.Empty) 
                              && !recordNameSanitized.Equals(string.Empty) 
                              && fileNameSanitized.Contains(recordNameSanitized);
                    
            // var mediaAnalysis = FFProbe.AnalyseAsync(file.Path).Result;
            var fileDuration = mediaAnalysis.Format.Duration.TotalSeconds;
            var recordDuration = recording.Duration;
            var matchesDuration = fileDuration > 0 
                                  && recordDuration > 0 
                                  && Math.Abs(recordDuration - fileDuration) < 10;
            
            if (strictMatch && matchesName && matchesDuration)
            {
                fingerPrintRecording = recording;
                break;
            }

            if (!matchesName && !matchesDuration) continue;
            fingerPrintRecording = recording;
            break;
        }
        
        return fingerPrintRecording;
    }
    
    private static string MakeArtistFolder(string artist)
    {
        var artistName = artist.RemoveDiacritics();

        var artistFolder = char.IsNumber(artistName[0])
            ? "#"
            : artistName[0].ToString().ToUpper();

        return $"/{artistFolder}/{artistName}";
    }
    
    
    private async Task<MusicBrainzReleaseAppends?> StoreReleaseGroups(MusicBrainzReleaseAppends musicBrainzRelease)
    {
        MediaContext mediaContext = new();
        var hasReleaseGroup = mediaContext.ReleaseGroups
            .AsNoTracking()
            .Any(r => r.Id == musicBrainzRelease.MusicBrainzReleaseGroup.Id);
        
        if (hasReleaseGroup) return null;
        
        ReleaseGroup insert = new()
        {
            Id = musicBrainzRelease.MusicBrainzReleaseGroup.Id,
            Title = musicBrainzRelease.MusicBrainzReleaseGroup.Title,
            Description = musicBrainzRelease.MusicBrainzReleaseGroup.Disambiguation.IsNullOrEmpty() ? null : musicBrainzRelease.MusicBrainzReleaseGroup.Disambiguation,
            Year = musicBrainzRelease.MusicBrainzReleaseGroup.FirstReleaseDate.ParseYear(),
            LibraryId = Library.Id,
        };

        try
        {
            await mediaContext.ReleaseGroups.Upsert(insert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new ReleaseGroup
                {
                    UpdatedAt = DateTime.UtcNow,
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Year = i.Year,
                    LibraryId = i.LibraryId
                })
                .RunAsync();
            
            foreach (var genre in musicBrainzRelease.MusicBrainzReleaseGroup.Genres ?? [])
            {
                await LinkGenreToReleaseGroup(musicBrainzRelease.MusicBrainzReleaseGroup, genre);
            }
            
            var musicDescriptionJob = new MusicDescriptionJob(musicBrainzRelease.MusicBrainzReleaseGroup);
            JobDispatcher.Dispatch(musicDescriptionJob, "data", 2);

        }
        catch (Exception e)
        {
            Logger.MusicBrainz(e, LogLevel.Error);
        }
        
        return musicBrainzRelease;
    }

    private async Task<MusicBrainzReleaseAppends?> StoreRelease(MusicBrainzReleaseAppends musicBrainzRelease, MediaFile mediaFile)
    {
        var media = musicBrainzRelease.Media.FirstOrDefault(m => m.Tracks.Length > 0);
        if (media is null) return null;
        
        MediaContext mediaContext = new();
        var hasAlbum = mediaContext.Albums
            .AsNoTracking()
            .Any(a => a.Id == musicBrainzRelease.Id && a.Cover != null);
        
        if (hasAlbum) return musicBrainzRelease;

        var folder = mediaFile.Parsed?.FilePath.Replace(Path.DirectorySeparatorChar + mediaFile.Name, "") ?? string.Empty;

        Album insert = new()
        {
            Id = musicBrainzRelease.Id,
            Name = musicBrainzRelease.Title,
            Country = musicBrainzRelease.Country,
            Disambiguation = musicBrainzRelease.Disambiguation.IsNullOrEmpty() ? null : musicBrainzRelease.Disambiguation,
            Year = musicBrainzRelease.DateTime?.ParseYear() ?? musicBrainzRelease.ReleaseEvents?.FirstOrDefault()?.DateTime?.ParseYear() ?? 0,
            Tracks = media.Tracks.Length,
            
            LibraryId = Library.Id,
            FolderId = Folder!.Id,
            Folder = folder.Replace(Folder.Path, "")
                .Replace("\\", "/"),
            HostFolder = folder.PathName(),
            
        };
        
        try
        {
            await mediaContext.Albums.Upsert(insert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new Album
                {
                    UpdatedAt = DateTime.UtcNow,
                    Id = i.Id,
                    Name = i.Name,
                    Disambiguation = i.Disambiguation,
                    Description = i.Description,
                    Year = i.Year,
                    Country = i.Country,
                    Tracks = i.Tracks,
                    
                    LibraryId = i.LibraryId,
                    Folder = i.Folder,
                    FolderId = i.FolderId,
                    HostFolder = i.HostFolder,
                })
                .RunAsync();

            foreach (var genre in musicBrainzRelease.Genres)
            {
                await LinkGenreToRelease(musicBrainzRelease, genre);
            }
            
            var coverArtImageJob = new CoverArtImageJob(musicBrainzRelease);
            JobDispatcher.Dispatch(coverArtImageJob, "image", 3);
            
            var fanartImagesJob = new FanartImagesJob(musicBrainzRelease);
            JobDispatcher.Dispatch(fanartImagesJob, "image", 2);
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["music", "albums", musicBrainzRelease.Id.ToString()]
            });

        }
        catch (Exception e)
        {
            Logger.MusicBrainz(e, LogLevel.Error);
        }

        return musicBrainzRelease;
    }

    private async Task StoreArtist(MusicBrainzArtistDetails musicBrainzArtist)
    {
        MediaContext mediaContext = new();
        var hasArtist = mediaContext.Artists
            .AsNoTracking()
            .Any(a => a.Id == musicBrainzArtist.Id);
        
        if (hasArtist) return;
        
        var artistFolder = MakeArtistFolder(musicBrainzArtist.Name);
        Artist insert = new()
        {
            Id = musicBrainzArtist.Id,
            Name = musicBrainzArtist.Name,
            Disambiguation = musicBrainzArtist.Disambiguation.IsNullOrEmpty() ? null : musicBrainzArtist.Disambiguation,
            Country = musicBrainzArtist.Country,
            TitleSort = musicBrainzArtist.SortName,

            Folder = artistFolder,
            HostFolder = Path.Join(Library.FolderLibraries.FirstOrDefault()?.Folder.Path, artistFolder).PathName(),
            LibraryId = Library.Id,
            FolderId = Folder!.Id,
        };
        
        try
        {
            await mediaContext.Artists.Upsert(insert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new Artist
                {
                    UpdatedAt = DateTime.UtcNow,
                    Id = i.Id,
                    Name = i.Name,
                    Disambiguation = i.Disambiguation,
                    Description = i.Description,
                    
                    Folder = i.Folder,
                    HostFolder = i.HostFolder,
                    LibraryId = i.LibraryId,
                    FolderId = i.FolderId,
                })
                .RunAsync();
            
            foreach (var genre in musicBrainzArtist.Genres ?? [])
            {
                await LinkGenreToArtist(musicBrainzArtist, genre);
            }
            
            var musicDescriptionJob = new MusicDescriptionJob(musicBrainzArtist);
            JobDispatcher.Dispatch(musicDescriptionJob, "data", 2);
            
            var fanartImagesJob = new FanartImagesJob(musicBrainzArtist);
            JobDispatcher.Dispatch(fanartImagesJob, "image", 2);
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["music", "artists", musicBrainzArtist.Id.ToString()]
            });
        }
        catch (Exception e)
        {
            Logger.MusicBrainz(e, LogLevel.Error);
        }
    }

    private async Task<MusicBrainzTrack?> StoreTrack(MusicBrainzReleaseAppends musicBrainzRelease, MusicBrainzTrack musicBrainzTrack, MusicBrainzMedia musicBrainzMedia, MediaFile mediaFile)
    {
        MediaContext mediaContext = new();
        var hasTrack = mediaContext.Tracks
            .AsNoTracking()
            .Any(t => t.Id == musicBrainzTrack.Id && t.Filename != null && t.Duration != null);
        
        if (hasTrack) return null;
        
        Track insert = new()
        {
            Id = musicBrainzTrack.Id,
            Name = musicBrainzTrack.Title,
            Date = musicBrainzRelease.DateTime ?? musicBrainzRelease.ReleaseEvents?.FirstOrDefault()?.DateTime,
            DiscNumber = musicBrainzMedia.Position,
            TrackNumber = musicBrainzTrack.Position,
        };
        
        var file = FileMatch(musicBrainzRelease, musicBrainzMedia, insert);

        if (file is not null)
        {
            var mediaAnalysis = FFProbe.AnalyseAsync(file).Result;
            var folder = mediaFile.Parsed?.FilePath.Replace(Path.DirectorySeparatorChar + mediaFile.Name, "") ??
                         string.Empty;

            insert.Filename = "/" + Path.GetFileName(file);
            insert.Quality = (int)Math.Floor((mediaAnalysis.Format.BitRate) / 1000.0);
            insert.Duration =
                HmsRegex().Replace((mediaAnalysis.Duration).ToString("hh\\:mm\\:ss"), "");

            insert.FolderId = Folder!.Id;
            insert.Folder = folder.Replace(Folder.Path, "").Replace("\\", "/");
            insert.HostFolder = folder.PathName();
        }

        try
        {
            await mediaContext.Tracks.Upsert(insert)
                .On(e => e.Id)
                .WhenMatched((ts, ti) => new Track
                {
                    UpdatedAt = DateTime.UtcNow,
                    Id = ti.Id,
                    Name = ti.Name,
                    DiscNumber = ti.DiscNumber,
                    TrackNumber = ti.TrackNumber,
                    Date = ti.Date,

                    Folder = file == "" ? ts.Folder : ti.Folder,
                    FolderId = file == "" ? ts.FolderId : ti.FolderId,
                    HostFolder = file == "" ? ts.HostFolder : ti.HostFolder,
                    Duration = file == "" ? ts.Duration : ti.Duration,
                    Filename = file == "" ? ts.Filename : ti.Filename,
                    Quality = file == "" ? ts.Quality : ti.Quality,
                    
                })
                .RunAsync();
            
            foreach (var genre in musicBrainzTrack.Genres ?? [])
            {
                await LinkGenreToTrack(musicBrainzTrack, genre);
            }

            return musicBrainzTrack;
        }
        catch (Exception e)
        {
            Logger.MusicBrainz(e, LogLevel.Error);
        }

        return null;
    }

    private string? FileMatch(MusicBrainzReleaseAppends musicBrainzRelease, MusicBrainzMedia musicBrainzMedia, Track track)
    {
        var file = FindTrackWithAlbumNumberByNumberPadded(musicBrainzMedia, null, musicBrainzRelease.Media.Length, 
            track.TrackNumber, 4);
        file = FindTrackWithAlbumNumberByNumberPadded(musicBrainzMedia, file, musicBrainzRelease.Media.Length, 
            track.TrackNumber, 3);
        file = FindTrackWithAlbumNumberByNumberPadded(musicBrainzMedia, file,musicBrainzRelease.Media.Length, 
            track.TrackNumber);

        file = FindTrackWithoutAlbumNumberByNumberPadded(musicBrainzMedia, file, musicBrainzRelease.Media.Length,
            track.TrackNumber, 4);
        file = FindTrackWithoutAlbumNumberByNumberPadded(musicBrainzMedia, file, musicBrainzRelease.Media.Length,
            track.TrackNumber, 3);
        file = FindTrackWithoutAlbumNumberByNumberPadded(musicBrainzMedia, file, musicBrainzRelease.Media.Length,
            track.TrackNumber);

        return file;
    }

    private async Task LinkReleaseToReleaseGroup(MusicBrainzReleaseAppends musicBrainzRelease)
    {
        AlbumReleaseGroup insert = new()
        {
            AlbumId = musicBrainzRelease.Id,
            ReleaseGroupId = musicBrainzRelease.MusicBrainzReleaseGroup.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.AlbumReleaseGroup.Upsert(insert)
            .On(e => new { e.AlbumId, e.ReleaseGroupId })
            .WhenMatched((s, i) => new AlbumReleaseGroup
            {
                AlbumId = i.AlbumId,
                ReleaseGroupId = i.ReleaseGroupId
            })
            .RunAsync();
    }

    private async Task LinkArtistToReleaseGroup(MusicBrainzReleaseAppends musicBrainzRelease, Guid artistId)
    {
        ArtistReleaseGroup insert = new()
        {
            ArtistId = artistId,
            ReleaseGroupId = musicBrainzRelease.MusicBrainzReleaseGroup.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.ArtistReleaseGroup.Upsert(insert)
            .On(e => new { e.ArtistId, e.ReleaseGroupId })
            .WhenMatched((s, i) => new ArtistReleaseGroup
            {
                ArtistId = i.ArtistId,
                ReleaseGroupId = i.ReleaseGroupId
            })
            .RunAsync();
    }

    private async Task LinkReleaseToLibrary(MusicBrainzReleaseAppends musicBrainzRelease)
    {
        AlbumLibrary insert = new()
        {
            AlbumId = musicBrainzRelease.Id,
            LibraryId = Library.Id
        };

        MediaContext mediaContext = new();
        await mediaContext.AlbumLibrary.Upsert(insert)
            .On(e => new { e.AlbumId, e.LibraryId })
            .WhenMatched((s, i) => new AlbumLibrary
            {
                AlbumId = i.AlbumId,
                LibraryId = i.LibraryId
            })
            .RunAsync();
    }

    private async Task LinkArtistToLibrary(MusicBrainzArtist musicBrainzArtistMusicBrainzArtist)
    {
        ArtistLibrary insert = new()
        {
            ArtistId = musicBrainzArtistMusicBrainzArtist.Id,
            LibraryId = Library.Id
        };

        MediaContext mediaContext = new();
        await mediaContext.ArtistLibrary.Upsert(insert)
            .On(e => new { e.ArtistId, e.LibraryId })
            .WhenMatched((s, i) => new ArtistLibrary
            {
                ArtistId = i.ArtistId,
                LibraryId = i.LibraryId
            })
            .RunAsync();
    }

    private async Task LinkTrackToRelease(MusicBrainzTrack? track, MusicBrainzReleaseAppends? release)
    {
        if (track == null || release == null) return;

        AlbumTrack insert = new()
        {
            AlbumId = release.Id,
            TrackId = track.Id
        };

        MediaContext mediaContext = new();
        await mediaContext.AlbumTrack.Upsert(insert)
            .On(e => new { e.AlbumId, e.TrackId })
            .WhenMatched((s, i) => new AlbumTrack
            {
                AlbumId = i.AlbumId,
                TrackId = i.TrackId
            })
            .RunAsync();

    }

    private async Task LinkArtistToAlbum(MusicBrainzArtist musicBrainzArtistMusicBrainzArtist, MusicBrainzReleaseAppends musicBrainzRelease)
    {
        AlbumArtist insert = new()
        {
            AlbumId = musicBrainzRelease.Id,
            ArtistId = musicBrainzArtistMusicBrainzArtist.Id
        };

        MediaContext mediaContext = new();
        await mediaContext.AlbumArtist.Upsert(insert)
            .On(e => new { e.AlbumId, e.ArtistId })
            .WhenMatched((s, i) => new AlbumArtist
            {
                AlbumId = i.AlbumId,
                ArtistId = i.ArtistId
            })
            .RunAsync();

    }

    private async Task LinkArtistToTrack(MusicBrainzArtist musicBrainzArtistMusicBrainzArtist, MusicBrainzTrack musicBrainzTrack)
    {
        ArtistTrack insert = new()
        {
            ArtistId = musicBrainzArtistMusicBrainzArtist.Id,
            TrackId = musicBrainzTrack.Id
        };

        MediaContext mediaContext = new();
        await mediaContext.ArtistTrack.Upsert(insert)
            .On(e => new { e.ArtistId, e.TrackId })
            .WhenMatched((s, i) => new ArtistTrack
            {
                ArtistId = i.ArtistId,
                TrackId = i.TrackId
            })
            .RunAsync();

    }
    
    private async Task LinkGenreToReleaseGroup(MusicBrainzReleaseGroup musicBrainzReleaseGroup, MusicBrainzGenreDetails musicBrainzGenre)
    {
        MusicGenreReleaseGroup insert = new()
        {
            GenreId = musicBrainzGenre.Id,
            ReleaseGroupId = musicBrainzReleaseGroup.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.MusicGenreReleaseGroup.Upsert(insert)
            .On(e => new { e.GenreId, e.ReleaseGroupId })
            .WhenMatched((s, i) => new MusicGenreReleaseGroup
            {
                GenreId = i.GenreId,
                ReleaseGroupId = i.ReleaseGroupId
            })
            .RunAsync();
    }
    
    private async Task LinkGenreToArtist(MusicBrainzArtistDetails musicBrainzArtist, MusicBrainzGenreDetails musicBrainzGenre)
    {
        ArtistMusicGenre insert = new()
        {
            MusicGenreId = musicBrainzGenre.Id,
            ArtistId = musicBrainzArtist.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.ArtistMusicGenre.Upsert(insert)
            .On(e => new { e.MusicGenreId, e.ArtistId })
            .WhenMatched((s, i) => new ArtistMusicGenre
            {
                MusicGenreId = i.MusicGenreId,
                ArtistId = i.ArtistId
            })
            .RunAsync();
    }
    
    private async Task LinkGenreToRelease(MusicBrainzReleaseAppends artist, MusicBrainzGenreDetails musicBrainzGenre)
    {
        AlbumMusicGenre insert = new()
        {
            MusicGenreId = musicBrainzGenre.Id,
            AlbumId = artist.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.AlbumMusicGenre.Upsert(insert)
            .On(e => new { e.MusicGenreId, e.AlbumId })
            .WhenMatched((s, i) => new AlbumMusicGenre
            {
                MusicGenreId = i.MusicGenreId,
                AlbumId = i.AlbumId
            })
            .RunAsync();
    }
    
    private async Task LinkGenreToTrack(MusicBrainzTrack musicBrainzTrack, MusicBrainzGenreDetails musicBrainzGenre)
    {
        MusicGenreTrack insert = new()
        {
            GenreId = musicBrainzGenre.Id,
            TrackId = musicBrainzTrack.Id
        };
        
        MediaContext mediaContext = new();
        await mediaContext.MusicGenreTrack.Upsert(insert)
            .On(e => new { e.GenreId, e.TrackId })
            .WhenMatched((s, i) => new MusicGenreTrack
            {
                GenreId = i.GenreId,
                TrackId = i.TrackId
            })
            .RunAsync();
    }
    
    private string? FindTrackWithoutAlbumNumberByNumberPadded(MusicBrainzMedia musicBrainzMedia, string? file, int numberOfAlbums, int trackNumber, int padding = 2)
    {
        if (file is not null) return file;
        if (numberOfAlbums > 1) return file;

        return Files?.FirstOrDefault(f =>
        {
            var fileName = Path.GetFileName(f.Parsed!.FilePath).RemoveDiacritics().RemoveNonAlphaNumericCharacters().ToLower();
                
            var matchNumber = $"{trackNumber.ToString().PadLeft(padding, '0')} ";
            var matchString = musicBrainzMedia.Tracks[trackNumber - 1].Title
                .RemoveDiacritics().RemoveNonAlphaNumericCharacters().ToLower().Replace(".mp3", "");
                
            return fileName.StartsWith(matchNumber)
                   && fileName.Contains(matchString);
        })?.Parsed!.FilePath;
    }

    private string? FindTrackWithAlbumNumberByNumberPadded(MusicBrainzMedia musicBrainzMedia, string? file, int numberOfAlbums, int trackNumber, int padding = 2)
    {
        if (file is not null) return file;
        if (numberOfAlbums == 1) return file;
        
        return Files?.FirstOrDefault(f =>
        {
            var fileName = Path.GetFileName(f.Parsed!.FilePath).RemoveDiacritics().RemoveNonAlphaNumericCharacters().ToLower();
                
            var matchNumber = $"{musicBrainzMedia.Position}-{trackNumber.ToString().PadLeft(padding, '0')} ";
            var matchString = musicBrainzMedia.Tracks[trackNumber - 1].Title
                .RemoveDiacritics().RemoveNonAlphaNumericCharacters().ToLower().Replace(".mp3", "");
                
            return fileName.StartsWith(matchNumber)
                   && fileName.Contains(matchString);
        })?.Parsed!.FilePath;
    }

    [GeneratedRegex("^00:")]
    private static partial Regex HmsRegex();

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    [GeneratedRegex(@"(?<library_folder>.+?)[\\\/]((?<letter>.{1})?|\[(?<type>.+?)\])[\\\/](?<artist>.+?)?[\\\/]?(\[(?<year>\d{4})\]?\s?(?<album>.*)?)")]
    private static partial Regex PathRegex();
}