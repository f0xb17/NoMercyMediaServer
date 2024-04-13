using System.Text.RegularExpressions;
using FFMpegCore;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.CoverArt.Client;
using NoMercy.Providers.CoverArt.Models;
using NoMercy.Providers.FanArt.Client;
using NoMercy.Providers.MusicBrainz.Client;
using NoMercy.Providers.MusicBrainz.Models;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using Album = NoMercy.Database.Models.Album;
using Artist = NoMercy.Database.Models.Artist;
using LogLevel = NoMercy.Helpers.LogLevel;
using Media = NoMercy.Providers.MusicBrainz.Models.Media;
using Track = NoMercy.Database.Models.Track;
using Release = NoMercy.Database.Models.Release;
using MusicBrainzTrack = NoMercy.Providers.MusicBrainz.Models.Track;
using MusicBrainzArtist = NoMercy.Providers.MusicBrainz.Models.Artist;
using MusicBrainzRelease = NoMercy.Providers.MusicBrainz.Models.Release;

namespace NoMercy.Server.Logic;

public partial class MusicLogic(Guid recordingId, Library library, string file, IMediaAnalysis mediaAnalysis)
{
    private readonly RecordingClient _recordingClient = new(recordingId);

    private RecordingAppends Music { get; set; } = new();

    private string File { get; set; } = file;
    private string Path { get; set; } = "";
    private string Folder { get; set; } = "";
    private string HostFolder { get; set; } = "";
    
    private string ArtistFolder { get; set; } = "";
    private string SanitizedArtistFolderName { get; set; } = "";
    
    private string AlbumFolder { get; set; } = "";
    private string SanitizedAlbumFolderName { get; set; } = "";
    
    private string FileName { get; set; } = "";
    private string SanitizedFileName { get; set; } = "";
    
    private string SanitizedFile { get; set; } = "";
    private Ulid FolderId { get; set; }
    
    private int? DiscNumber { get; set; }
    private int? TrackNumber { get; set; }

    private IMediaAnalysis MediaAnalysis { get; set; } = mediaAnalysis;

    public async Task Process()
    {
        Logger.App($"Processing {File}");

        RecordingAppends? music = await _recordingClient.WithAllAppends();

        if (music is null) return;
        Music = music;

        await using MediaContext mediaContext = new();
        FolderId = mediaContext.Folders
            .First(e => File.Contains(e.Path)).Id;

        Path = File.Replace(library.FolderLibraries
            .FirstOrDefault()?.Folder.Path ?? "", "");

        Match reg = Regex.Match(Path, "(([\\\\\\/].+)([\\\\\\/].+))([\\\\\\/].+)");
        Folder = reg.Groups[1].Value.Replace("\\", "/");
        ArtistFolder = reg.Groups[2].Value.Replace("\\", "/");
        AlbumFolder = reg.Groups[3].Value.Replace("\\", "/");
        
        FileName = reg.Groups[4].Value.Replace("\\", "/");

        HostFolder = (library.FolderLibraries
            .FirstOrDefault()?.Folder.Path + Folder).Replace("/", "\\");
        
        SanitizedFile = MyRegex()
            .Replace(File.ToLower(), "")
            .RemoveDiacritics();
        
        SanitizedAlbumFolderName = MyRegex()
            .Replace(AlbumFolder.ToLower(), "")
            .RemoveDiacritics();
        
        SanitizedArtistFolderName = MyRegex()
            .Replace(ArtistFolder.ToLower(), "")
            .Replace("/", "")
            .RemoveDiacritics();
        
        SanitizedFileName = MyRegex()
            .Replace(FileName.ToLower(), "")
            .RemoveDiacritics();
        
        if(MediaAnalysis?.Format.Tags?.ContainsKey("disc") != null)
            DiscNumber = int.Parse(MediaAnalysis.Format.Tags["disc"].Split("/")[0]);
        
        if(MediaAnalysis?.Format.Tags?.ContainsKey("track") != null)
            TrackNumber = int.Parse(MediaAnalysis.Format.Tags["track"].Split("/")[0]);
        
        await Store();

        await DispatchJobs();
    }
    
    private string MakeArtistFolder()
    {
        string artistName = Music.ArtistCredit.FirstOrDefault()?.Name
            .Replace("/", "_")
            .Replace("“", "")
            .Replace("^\\.+", "")
            .Replace("[\"*?<>|]", "") ?? "";

        string artistFolder = char.IsNumber(artistName[0])
            ? "#"
            : artistName[0].ToString().ToUpper();

        return $"{artistFolder}/{artistName}/";
    }

    public async Task StoreImages(int id)
    {
        await Task.CompletedTask;
    }
    
    [GeneratedRegex("([^\\s\\\\\\/\\.a-zA-Z0-9-])")]
    private static partial Regex MyRegex();

    [GeneratedRegex("^00:")]
    private static partial Regex HmsRegex();
    
    private async Task Store()
    {
        if (Music.Releases.Length == 0) return;

        // Release releases = Music.Releases
        //     .Where(r =>
        //     {
        //         MusicBrainzRelease title = r.Title.ToLower();
        //         string san = MyRegex()
        //             .Replace(title, "")
        //             .RemoveDiacritics();
        //         return SanitizedFile.Contains(san);
        //     })
        //     .ToList();
        
        foreach (MusicBrainzRelease release in Music.Releases)
        // foreach (MusicBrainzRelease release in Music.Releases.Where(r => r.Title != ""))
        {
            if(release.Id == Guid.Empty) continue;
            
            ReleaseClient releaseClient = new(release.Id);
            ReleaseAppends? releaseAppends = await releaseClient.WithAllAppends();

            if (releaseAppends is null) return;
            // if(DiscNumber is not null && TrackNumber is not null)
            // {
            //     if(releaseAppends.Media.All(m => m.Position != DiscNumber)) return;
            //     if(releaseAppends.Media.All(m => m.Tracks.All(t => t.Position != TrackNumber))) return;
            //     
            //     releaseAppends.Media = releaseAppends.Media
            //         .Where(m => 
            //             m.Position == DiscNumber && 
            //             m.Tracks.Any(t => t.Position == TrackNumber)
            //         ).ToArray();
            // }

            string cover = "";
     
            bool hasCover = releaseAppends.CoverArtArchive.Front;
            if (hasCover)
            {
                CoverArtClient coverArtClient = new(releaseAppends.Id);
                Covers? covers = await coverArtClient.Cover();
                if (covers is null) return;

                List<CoverImage> coverList = covers.Images
                    .Where(image => image.Types.Contains("Front"))
                    .ToList();

                foreach (CoverImage coverItem in coverList)
                {
                    string url = coverItem.Thumbnails.Large
                        .Replace("http://", "https://");

                    HttpClient httpClient = new();
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "NoMercy");
                    httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

                    HttpResponseMessage res = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

                    if (!res.IsSuccessStatusCode) continue;

                    cover = url;
                    break;
                }
            }
            
            await StoreReleaseGroups(releaseAppends, cover);

            await StoreAlbum(releaseAppends, cover);
            await LinkAlbumToLibrary(releaseAppends);
            
            foreach (Media media in releaseAppends.Media)
            {
                foreach (MusicBrainzTrack track in media.Tracks)
                {
                    if (await StoreTrack(releaseAppends, track, media, cover) is null) continue;
                    
                    await LinkTrackToAlbum(track, release);

                    foreach (ReleaseArtistCredit artist in track.ArtistCredit)
                    {
                        await StoreArtist(artist.Artist);
                        await LinkArtistToTrack(artist.Artist, track);
                        
                        await LinkArtistToAlbum(artist.Artist, releaseAppends);
                        await LinkArtistToLibrary(artist.Artist);
                        
                        await LinkArtistToReleaseGroup(releaseAppends, artist.Artist.Id);
                    }
                }
            }
        }
    }
    
    private async Task StoreReleaseGroups(MusicBrainzRelease release, string? cover)
    {
        Release releaseInsert = new()
        {
            Id = release.ReleaseGroup.Id,
            Title = release.ReleaseGroup.Title,
            Description = release.ReleaseGroup.Disambiguation,
            Year = release.ReleaseGroup.FirstReleaseDate.ParseYear(),
            LibraryId = library.Id,
            Cover = cover,
        };
        
        try
        {
            await using MediaContext mediaContext = new();
            await mediaContext.Releases.Upsert(releaseInsert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new Release
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Year = i.Year,
                    Cover = s.Cover ?? i.Cover,
                    LibraryId = i.LibraryId,
                })
                .RunAsync();
            
            await StoreReleaseImages(release);

        }
        catch (Exception e)
        {
            Logger.App(e, LogLevel.Error);
            Logger.App(release);
        }
    }

    private async Task StoreAlbum(MusicBrainzRelease release, string? cover)
    {
        Media? media = release.Media.FirstOrDefault(m => m.Tracks.Length > 0);
        if (media is null) return;
        
        string sanitizedAlbumName = MyRegex()
            .Replace(release.Title.ToLower(), "")
            .RemoveDiacritics();

        bool shouldInsert = SanitizedAlbumFolderName.Contains(sanitizedAlbumName);

        await using MediaContext mediaContext = new();
        if (!shouldInsert && mediaContext.Albums.Any(a => a.Id == release.Id))
        {
            return;
        }

        Album albumInsert = new()
        {
            Id = release.Id,
            Name = release.Title,
            Country = release.Country,
            Description = release.Disambiguation,
            Year = release.Date.ParseYear(),
            Tracks = media.Tracks.Length,
            LibraryId = library.Id,
        };

        if (shouldInsert)
        {
            albumInsert.Cover = cover is not null ? $"/{release.Id}.jpg" : null;
            albumInsert.LibraryId = library.Id;
            albumInsert.FolderId = FolderId;
            albumInsert.Folder = Folder;
            albumInsert.HostFolder = HostFolder;
            albumInsert._colorPalette = await ImageLogic.GenerateColorPalette(coverPath: cover);
            
            await DownloadImage(cover, release.Id);
        }

        try
        {
            await mediaContext.Albums.Upsert(albumInsert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new Album
                {
                    Id = i.Id,
                    Cover = i.Cover,
                    Name = i.Name,
                    Description = i.Description,
                    Year = i.Year,
                    Country = i.Country,
                    Tracks = i.Tracks,

                    LibraryId = shouldInsert ? i.LibraryId : s.LibraryId,
                    Folder = shouldInsert ? i.Folder : s.Folder,
                    FolderId = shouldInsert ? i.FolderId : s.FolderId,
                    HostFolder = shouldInsert ? i.HostFolder : s.HostFolder,
                })
                .RunAsync();
            
            MusicColorPaletteJob musicColorPaletteJob = new(albumInsert.Id.ToString(), "album");
            await musicColorPaletteJob.Handle();

            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["music", "album", albumInsert.Id.ToString()]
            });
            
            await LinkAlbumToReleaseGroup(release);
        }
        catch (Exception e)
        {
            Logger.App(e, LogLevel.Error);
            Logger.App(albumInsert);
        }
    }

    private async Task StoreArtist(MusicBrainzArtist? artist)
    {
        if (artist == null) return;

        string sanitizedArtistName = MyRegex()
            .Replace(artist.Name.ToLower(), "")
            .RemoveDiacritics();
        
        bool shouldInsert = SanitizedArtistFolderName == sanitizedArtistName;
        
        await using MediaContext mediaContext = new();
        if (!shouldInsert && mediaContext.Artists.Any(a => a.Id == artist.Id))
        {
            return;
        }

        Artist artistInsert = new()
        {
            Id = artist.Id,
            Name = artist.Name,
            Description = artist.Disambiguation,
            LibraryId = library.Id,
        };

        if (shouldInsert)
        {
            string artistFolder = MakeArtistFolder();

            artistInsert.Folder = artistFolder;
            artistInsert.HostFolder = (library.FolderLibraries
                    .FirstOrDefault()?.Folder.Path + "\\" + artistFolder)
                .Replace("/", "\\");
            artistInsert.LibraryId = library.Id;
            artistInsert.FolderId = FolderId;
        }

        try
        {
            await mediaContext.Artists.Upsert(artistInsert)
                .On(e => e.Id)
                .WhenMatched((s, i) => new Artist
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    // Cover = i.Cover,
                    Folder = i.Folder,
                    HostFolder = i.HostFolder,

                    LibraryId = shouldInsert ? i.LibraryId : s.LibraryId,
                    FolderId = shouldInsert ? i.FolderId : s.FolderId,
                })
                .RunAsync();    
            
            MusicColorPaletteJob musicColorPaletteJob = new(artistInsert.Id.ToString(), "artist");
            await musicColorPaletteJob.Handle();

            await StoreArtistImages(artist);
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["music", "artist", artistInsert.Id.ToString()]
            });

        }
        catch (Exception e)
        {
            Logger.App(e, LogLevel.Error);
            Logger.App(artistInsert);
        }
    }

    private async Task<MusicBrainzTrack?> StoreTrack(MusicBrainzRelease release, MusicBrainzTrack track, Media media, string? cover)
    {
        string sanitizedAlbumName = MyRegex()
            .Replace(release.Title.ToLower(), "")
            .RemoveDiacritics();
        bool albumNameMatches = SanitizedAlbumFolderName.Contains(sanitizedAlbumName);
        
        string sanitizedTrackName = MyRegex()
            .Replace(track.Title.ToLower(), "")
            .RemoveDiacritics();
        bool trackNameMatches = SanitizedFileName.Contains(sanitizedTrackName);
        
        bool namesMatches = albumNameMatches && trackNameMatches;
        
        bool canUsePosition = DiscNumber is not null && TrackNumber is not null;
        
        bool albumPosition = media.Position == DiscNumber;
        bool trackPositionNumber = track.Position == TrackNumber;
        bool trackNumbersMatches = albumPosition && trackPositionNumber;
        
        // if(canUsePosition && !trackNumbersMatches) return null;

        bool shouldInsert = (canUsePosition && trackNumbersMatches && namesMatches) || (!canUsePosition && namesMatches);

        await using MediaContext mediaContext = new();
        if (!shouldInsert && mediaContext.Tracks.Any(t => t.Id == track.Id))
        {
            return null;
        }

        Track trackInsert = new()
        {
            Id = track.Id,
            Name = track.Title,
            Date = Music.FirstReleaseDate,
            DiscNumber = media.Position,
            TrackNumber = track.Position,
        };

        if (shouldInsert)
        {
            trackInsert.Quality = (int)Math.Floor((MediaAnalysis?.Format.BitRate ?? 0.0) / 1000.0);
            trackInsert.Duration = HmsRegex().Replace((MediaAnalysis?.Duration ?? new TimeSpan()).ToString("hh\\:mm\\:ss"), "");
            
            trackInsert.Cover = cover is not null ? $"/{release.Id}.jpg" : null;

            trackInsert.Filename = FileName;
            trackInsert.FolderId = FolderId;
            trackInsert.Folder = Folder;
            trackInsert.HostFolder = HostFolder;
            trackInsert._colorPalette = await ImageLogic.GenerateColorPalette(coverPath: cover);


            Logger.Http(
                $"{release.Title}: {media.Position}-{track.Number} / {track.ArtistCredit.FirstOrDefault()?.Name} - {track.Title}");
        }

        try
        {
            await mediaContext.Tracks.Upsert(trackInsert)
                .On(e => e.Id)
                .WhenMatched((ts, ti) => new Track
                {
                    Id = ti.Id,
                    Name = ti.Name,
                    DiscNumber = ti.DiscNumber,
                    TrackNumber = ti.TrackNumber,
                    Cover = ti.Cover,
                    Date = ti.Date,

                    Duration = shouldInsert ? ti.Duration : ts.Duration,
                    Filename = shouldInsert ? ti.Filename : ts.Filename,
                    Folder = shouldInsert ? ti.Folder : ts.Folder,
                    FolderId = shouldInsert ? ti.FolderId : ts.FolderId,
                    HostFolder = shouldInsert ? ti.HostFolder : ts.HostFolder,
                    Quality = shouldInsert ? ti.Quality : ts.Quality,
                })
                .RunAsync();

            if (!shouldInsert) return track;
            
            MusicColorPaletteJob musicColorPaletteJob = new(trackInsert.Id.ToString(), "track");
            await musicColorPaletteJob.Handle();

            return track;
        }
        catch (Exception e)
        {
            Logger.App(e, LogLevel.Error);
            Logger.App(trackInsert);
        }
        
        return null;
    }

    private static async Task LinkAlbumToReleaseGroup(MusicBrainzRelease release)
    {
        AlbumRelease albumRelease = new()
        {
            AlbumId = release.Id,
            ReleaseId = release.ReleaseGroup.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.AlbumRelease.Upsert(albumRelease)
            .On(e => new { e.AlbumId, e.ReleaseId })
            .WhenMatched((s, i) => new AlbumRelease
            {
                AlbumId = i.AlbumId,
                ReleaseId = i.ReleaseId
            })
            .RunAsync();
    }
    
    private static async Task LinkArtistToReleaseGroup(MusicBrainzRelease release, Guid artistId)
    {
        ArtistRelease artistRelease = new()
        {
            ArtistId = artistId,
            ReleaseId = release.ReleaseGroup.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.ArtistRelease.Upsert(artistRelease)
            .On(e => new { e.ArtistId, e.ReleaseId })
            .WhenMatched((s, i) => new ArtistRelease
            {
                ArtistId = i.ArtistId,
                ReleaseId = i.ReleaseId
            })
            .RunAsync();
    }
    
    private async Task LinkAlbumToLibrary(MusicBrainzRelease release)
    {
        AlbumLibrary albumLibrary = new()
        {
            AlbumId = release.Id,
            LibraryId = library.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.AlbumLibrary.Upsert(albumLibrary)
            .On(e => new { e.AlbumId, e.LibraryId })
            .WhenMatched((s, i) => new AlbumLibrary
            {
                AlbumId = i.AlbumId,
                LibraryId = i.LibraryId
            })
            .RunAsync();
    }

    private async Task LinkArtistToLibrary(MusicBrainzArtist artistArtist)
    {
        ArtistLibrary artistLibrary = new()
        {
            ArtistId = artistArtist.Id,
            LibraryId = library.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.ArtistLibrary.Upsert(artistLibrary)
            .On(e => new { e.ArtistId, e.LibraryId })
            .WhenMatched((s, i) => new ArtistLibrary
            {
                ArtistId = i.ArtistId,
                LibraryId = i.LibraryId,
            })
            .RunAsync();
    }

    private static async Task LinkTrackToAlbum(MusicBrainzTrack? track, MusicBrainzRelease? release)
    {
        if (track == null || release == null) return;

        AlbumTrack albumTrack = new()
        {
            AlbumId = release.Id,
            TrackId = track.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.AlbumTrack.Upsert(albumTrack)
            .On(e => new { e.AlbumId, e.TrackId })
            .WhenMatched((s, i) => new AlbumTrack
            {
                AlbumId = i.AlbumId,
                TrackId = i.TrackId,
            })
            .RunAsync();

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
        {
            QueryKey = ["music", "album", albumTrack.AlbumId.ToString()]
        });
    }

    private static async Task LinkArtistToAlbum(MusicBrainzArtist artistArtist, MusicBrainzRelease release)
    {
        AlbumArtist albumArtist = new()
        {
            AlbumId = release.Id,
            ArtistId = artistArtist.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.AlbumArtist.Upsert(albumArtist)
            .On(e => new { e.AlbumId, e.ArtistId })
            .WhenMatched((s, i) => new AlbumArtist
            {
                AlbumId = i.AlbumId,
                ArtistId = i.ArtistId,
            })
            .RunAsync();

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
        {
            QueryKey = ["music", "artist", albumArtist.ArtistId.ToString()]
        });
    }

    private static async Task LinkArtistToTrack(MusicBrainzArtist artistArtist, MusicBrainzTrack track)
    {
        ArtistTrack artistTrack = new()
        {
            ArtistId = artistArtist.Id,
            TrackId = track.Id,
        };

        await using MediaContext mediaContext = new();
        await mediaContext.ArtistTrack.Upsert(artistTrack)
            .On(e => new { e.ArtistId, e.TrackId })
            .WhenMatched((s, i) => new ArtistTrack
            {
                ArtistId = i.ArtistId,
                TrackId = i.TrackId,
            })
            .RunAsync();

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
        {
            QueryKey = ["music", "artist", artistTrack.ArtistId.ToString()]
        });
    }
    
    private static async Task StoreArtistImages(MusicBrainzArtist artistArtist)
    {
        try
        {
            MusicClient musicClient = new();
            Providers.FanArt.Models.ArtistDetails? fanArt = await musicClient.Artist(artistArtist.Id);

            if (fanArt is null) return;
            
            List<Image> thumbs = fanArt.Thumbs.ToList()
                .ConvertAll<Image>(image => new Image(image, artistArtist, type: "thumb"));
            List<Image> logos = fanArt.Logos.ToList()
                .ConvertAll<Image>(image => new Image(image, artistArtist, type: "logo"));
            List<Image> banners = fanArt.Banners.ToList()
                .ConvertAll<Image>(image => new Image(image, artistArtist, type: "banner"));
            List<Image> hdLogos = fanArt.HdLogos.ToList()
                .ConvertAll<Image>(image => new Image(image, artistArtist, type: "hdLogo"));
            List<Image> artistBackgrounds = fanArt.Backgrounds.ToList()
                .ConvertAll<Image>(image => new Image(image, artistArtist, type: "background"));
                
            List<Image> images = thumbs
                .Concat(logos)
                .Concat(banners)
                .Concat(hdLogos)
                .Concat(artistBackgrounds)
                .ToList();
                
            await using MediaContext mediaContext = new();
            await mediaContext.Images.UpsertRange(images)
                .On(v => new { v.FilePath, v.ArtistId })
                .WhenMatched((s, i) => new Image
                {
                    Id = i.Id,
                    AspectRatio = i.AspectRatio,
                    Height = i.Height,
                    Iso6391 = i.Iso6391,
                    FilePath = i.FilePath,
                    Width = i.Width,
                    VoteAverage = i.VoteAverage,
                    VoteCount = i.VoteCount,
                    ArtistId = i.ArtistId,
                    Type = i.Type,
                    Site = i.Site,
                })
                .RunAsync();

            string? url = await DownloadArtistImage(artistArtist, fanArt);

            if(url is not null)
            {
                string palette = await ImageLogic.GenerateColorPalette(coverPath: url);
                        
                Artist artist = await mediaContext.Artists
                    .FirstAsync(a => a.Id == artistArtist.Id);
                        
                artist.Cover = $"/{artistArtist.Id}.jpg";
                artist._colorPalette = palette;
                        
                await mediaContext.SaveChangesAsync();

            }
            
            MusicColorPaletteJob musicColorPaletteJob = new(artistArtist.Id.ToString(), "artist");
            JobDispatcher.Dispatch(musicColorPaletteJob, "data", 3);

        }
        catch (Exception)
        {
            //
            
            //TODO: var cover = crawl lastFM for artist image
            // await DownloadImage(cover, artist.Id);
        }
    }

    private static async Task StoreReleaseImages(MusicBrainzRelease release)
    {
        try
        {
            MusicClient musicClient = new();
            Providers.FanArt.Models.Album? fanArt = await musicClient.Album(release.ReleaseGroup.Id);
            
            if (fanArt is null) return;
            
            List<Image> cdArts = fanArt.Albums.SelectMany(a => a.Value.CdArt).ToList()
                .ConvertAll<Image>(image => new Image(image, release.ReleaseGroup, type: "cd"));
            List<Image> covers = fanArt.Albums.SelectMany(a => a.Value.Cover).ToList()
                .ConvertAll<Image>(image => new Image(image, release.ReleaseGroup, type: "cover"));
            
            List<Image> images = cdArts
                .Concat(covers)
                .ToList();
            
            await using MediaContext mediaContext = new();
            await mediaContext.Images.UpsertRange(images)
                .On(v => new { v.FilePath, v.ArtistId })
                .WhenMatched((s, i) => new Image
                {
                    Id = i.Id,
                    AspectRatio = i.AspectRatio,
                    Height = i.Height,
                    Iso6391 = i.Iso6391,
                    FilePath = i.FilePath,
                    Width = i.Width,
                    VoteAverage = i.VoteAverage,
                    VoteCount = i.VoteCount,
                    ArtistId = i.ArtistId,
                    Type = i.Type,
                    Site = i.Site,
                })
                .RunAsync();
            
            string? url = await DownloadAlbumImage(release, fanArt);
            
            if(url is not null)
            {
                string palette = await ImageLogic.GenerateColorPalette(coverPath: url);
                
                Album artist = await mediaContext.Albums
                    .FirstAsync(a => a.Id == release.Id);
                
                artist.Cover = $"/{release.Id}.jpg";
                artist._colorPalette = palette;
                
                await mediaContext.SaveChangesAsync();

            }
            
            MusicColorPaletteJob musicColorPaletteJob = new(release.Id.ToString(), "album");
            JobDispatcher.Dispatch(musicColorPaletteJob, "data", 3);
        }
        catch (Exception)
        {
            //
        }
    }

    private static async Task<string?> DownloadArtistImage(MusicBrainzArtist artistArtist, Providers.FanArt.Models.ArtistDetails fanArt)
    {
        string? url = fanArt.Thumbs.FirstOrDefault()?.Url.ToString()
            .Replace("http://", "https://");
        
        await DownloadImage(url, artistArtist.Id);
        
        return url;
    }
    
    private static async Task<string?> DownloadAlbumImage(MusicBrainzRelease release, Providers.FanArt.Models.Album fanArt)
    {
        string? url = fanArt.Albums.SelectMany(a => a.Value.Cover)
            .FirstOrDefault()?.Url.ToString()
            .Replace("http://", "https://");
        
        await DownloadImage(url, release.Id);

        return url;
    }

    private static async Task DownloadImage(string? url, Guid imagePath)
    {
        if (url is null) return;

        string fullPath = $"{AppFiles.MusicImagesPath}\\{imagePath}.jpg";
        
        if (System.IO.File.Exists(fullPath)) return;
        
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "NoMercy");
        httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

        HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                
        if (response.IsSuccessStatusCode)
        {
            await httpClient.GetByteArrayAsync(url)
                .ContinueWith(async task =>
                {
                    await System.IO.File.WriteAllBytesAsync(fullPath, await task);
                });
        }
    }

    private Task DispatchJobs()
    {
        return Task.CompletedTask;
    }

    public static async Task GetPalette(string id, string type)
    {
        await using MediaContext mediaContext = new();

        switch (type)
        {
            case "artist":
                Artist? artist = await mediaContext.Artists
                    .Where(e => e.Id == Guid.Parse(id))
                    .FirstOrDefaultAsync();

                if (artist is not { _colorPalette: "" }) return;
                
                string artistPalette = await ImageLogic.GenerateColorPalette(coverPath: artist.Cover);
                artist._colorPalette = artistPalette;
                
                break;
            case "album":
                var album = await mediaContext.Albums
                    .Where(e => e.Id == Guid.Parse(id))
                    .FirstOrDefaultAsync();

                    if (album is not { _colorPalette: "" }) return;
                    
                    string albumPalette = await ImageLogic.GenerateColorPalette(coverPath: album.Cover);
                    album._colorPalette = albumPalette;

                break;
            case "track":
                Track? track = await mediaContext.Tracks
                    .Where(e => e.Id == Guid.Parse(id))
                    .FirstOrDefaultAsync();

                    // if (track is not { _colorPalette: "" }) return;
                    if(track is null) return;
                    
                    string trackPalette = await ImageLogic.GenerateColorPalette(coverPath: track.Cover);
                    track._colorPalette = trackPalette;

                break;
        }

        await mediaContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        // Music = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }

}