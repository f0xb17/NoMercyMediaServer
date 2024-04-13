using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;

public class ArtistResponseDto
{
    [JsonProperty("data")] public ArtistResponseItemDto? Data { get; set; }
    
        public static readonly Func<MediaContext, Guid, Guid, Task<Artist?>> GetArtist =
            EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Guid id) => 
                mediaContext.Artists.AsNoTracking()

                .Where(album => album.Id == id)

                .OrderBy(artist => artist.Name)

                .Where(artist => artist.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)

                .Include(artist => artist.Library)

                .Include(artist => artist.ArtistUser
                    .Where(artistUser => artistUser.UserId == userId)
                )

                .Include(artist => artist.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Album)

                .Include(artist => artist.ArtistTrack
                        .OrderBy(albumTrack => albumTrack.Track.TrackNumber)
                        .ThenBy(albumTrack => albumTrack.Track.DiscNumber)
                    // .Where(artistTrack => artistTrack.Track.Duration != null)
                )

                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)

                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.TrackUser
                    .Where(trackUser => trackUser.UserId == userId)
                )
                .ThenInclude(trackUser => trackUser.User)

                .FirstOrDefault());
}

public class ArtistResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("country")] public string? Country { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    
    [JsonProperty("albums")] public IEnumerable<AlbumDto> Albums { get; set; }
    [JsonProperty("tracks")] public IEnumerable<ArtistTrackDto> Tracks { get; set; }

    public ArtistResponseItemDto(Artist artist)
    {
        ColorPalette = artist.ColorPalette;
        Cover = artist.Cover;
        Description = artist.Description;
        Favorite = artist.ArtistUser.Any();
        Folder = artist.Folder;
        Id = artist.Id;
        LibraryId = artist.LibraryId;
        Name = artist.Name;
        Type = "artists";
        
        Albums = artist.AlbumArtist
            .Select(trackAlbum => new AlbumDto(trackAlbum));
        
        var artists = Databases.MediaContext.ArtistTrack
            .Where(at => at.ArtistId == artist.Id)
            .Include(at => at.Artist)
            .Include(artistTrack => artistTrack.Track)
            .ThenInclude(track => track.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Album)
            .Include(artistTrack => artistTrack.Track)
            .ThenInclude(track => track.TrackUser)
            .ToList();
        
        Tracks = artists
            .DistinctBy(artistTrack => Regex.Replace(artistTrack.Track.Filename ?? "", @"[\d+-]\s", "").ToLower())
            // .DistinctBy(artistTrack => artistTrack.Track.Name.ToLower())
            .Select(albumTrack => new ArtistTrackDto(albumTrack));
    }

}

public class ArtistTrackDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
    [JsonProperty("disc")] public int? Disc { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("quality")] public int? Quality { get; set; }
    [JsonProperty("track")] public int? Track { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }
    
    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }
    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }

    public ArtistTrackDto(ArtistTrack artistTrack)
    {
        ColorPalette = artistTrack.Track.AlbumTrack.First().Album.ColorPalette ?? artistTrack.Track.ArtistTrack.First().Artist.ColorPalette;
        Cover = artistTrack.Track.AlbumTrack.First().Album.Cover ?? artistTrack.Track.ArtistTrack.First().Artist.Cover;
        Date = artistTrack.Track.Date;
        Disc = artistTrack.Track.DiscNumber;
        Duration = artistTrack.Track.Duration;
        Favorite = artistTrack.Track.TrackUser.Any();
        Filename = artistTrack.Track.Filename;
        Folder = artistTrack.Track.Folder;
        FolderId = artistTrack.Track.FolderId;
        Id = artistTrack.Track.Id;
        LibraryId = artistTrack.Artist.LibraryId;
        Name = artistTrack.Track.Name;
        Origin = SystemInfo.DeviceId;
        Path = artistTrack.Track.Folder + "/" + artistTrack.Track.Filename;
        Quality = artistTrack.Track.Quality;
        Track = artistTrack.Track.TrackNumber;
        Lyrics = artistTrack.Track.Lyrics;
        Type = "tracks";
        
        Album = artistTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack));
        
        var artists = Databases.MediaContext.ArtistTrack
            .Where(at => at.TrackId == artistTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();
        
        Artist = artists
            .Select(albumTrack => new ArtistDto(albumTrack));
    }

    public ArtistTrackDto(Track track)    {
        ColorPalette = track.AlbumTrack.First().Album.ColorPalette ?? track.ArtistTrack.First().Artist.ColorPalette;
        Cover = track.AlbumTrack.First().Album.Cover ?? track.ArtistTrack.First().Artist.Cover;
        Date = track.UpdatedAt;
        Disc = track.DiscNumber;
        Duration = track.Duration;
        Favorite = track.TrackUser.Any();
        Filename = track.Filename;
        Folder = track.Folder;
        FolderId = track.FolderId;
        Id = track.Id;
        LibraryId = track.AlbumTrack.First().Album.LibraryId;
        Name = track.Name;
        Origin = SystemInfo.DeviceId;
        Path = track.Folder + "/" + track.Filename;
        Quality = track.Quality;
        Track = track.TrackNumber;
        Type = "tracks";
        
        Album = track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack));
        
        Artist = track.ArtistTrack
            .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(trackArtist => new ArtistDto(trackArtist));

    }
}

public class ArtistDto
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }

    public ArtistDto(AlbumArtist albumArtist)
    {
        Id = albumArtist.Artist.Id;
        Name = albumArtist.Artist.Name;
        Description = albumArtist.Artist.Description;
        Cover = albumArtist.Artist.Cover;
        Folder = albumArtist.Artist.Folder;
        ColorPalette = albumArtist.Artist.ColorPalette;
        LibraryId = albumArtist.Artist.LibraryId;
        Origin = SystemInfo.DeviceId;
    }

    public ArtistDto(ArtistTrack artistTrack)
    {
        Id = artistTrack.Artist.Id;
        Name = artistTrack.Artist.Name;
        Description = artistTrack.Artist.Description;
        Cover = artistTrack.Artist.Cover;
        Folder = artistTrack.Artist.Folder;
        ColorPalette = artistTrack.Artist.ColorPalette;
        LibraryId = artistTrack.Artist.LibraryId;
        Origin = SystemInfo.DeviceId;
    }
}