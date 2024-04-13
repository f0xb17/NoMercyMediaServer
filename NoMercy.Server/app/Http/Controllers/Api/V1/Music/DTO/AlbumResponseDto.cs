using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;

public class AlbumResponseDto
{
    [JsonProperty("data")] public AlbumResponseItemDto Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, Guid, Task<Album?>> GetAlbum =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Guid id) => mediaContext.Albums
            .AsNoTracking()
            
            .Where(album => album.Id == id)
            
            .OrderBy(album => album.Name)
            
            .Where(album => album.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            
            .Include(album => album.Library)
            
            .Include(album => album.AlbumUser
                .Where(albumUser => albumUser.UserId == userId)
            )
            
            .Include(album => album.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Artist)
            
            
            .Include(album => album.AlbumTrack
                .OrderBy(albumTrack => albumTrack.Track.DiscNumber)
                .ThenBy(albumTrack => albumTrack.Track.TrackNumber)
                .Where(artistTrack => artistTrack.Track.Duration != null)
            )
            
            .Include(album => album.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Track)
                    .ThenInclude(track => track.TrackUser
                        .Where(albumUser => albumUser.UserId == userId)
                    )
            
            .Include(album => album.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Track)
                    .ThenInclude(track => track.ArtistTrack)
                        .ThenInclude(artistTrack => artistTrack.Artist)
            
            .FirstOrDefault());
        
}

public class AlbumResponseItemDto
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
    
    [JsonProperty("artists")] public IEnumerable<ArtistDto> Artists { get; set; }
    [JsonProperty("tracks")] public IEnumerable<AlbumTrackDto> Tracks { get; set; }

    public AlbumResponseItemDto(Album album)
    {
        ColorPalette = album.ColorPalette;
        Cover = album.Cover;
        Description = album.Description;
        Favorite = album.AlbumUser.Count != 0;
        Folder = album.Folder;
        Id = album.Id;
        LibraryId = album.LibraryId;
        Name = album.Name;
        Type = "albums";
        
        Artists = album.AlbumArtist
            .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(albumArtist => new ArtistDto(albumArtist));
        
        Tracks = album.AlbumTrack
            .DistinctBy(artistTrack => Regex.Replace(artistTrack.Track.Filename ?? "", @"\[\d+-]\s", "").ToLower())
            // .DistinctBy(artistTrack => artistTrack.Track.Name.ToLower())
            .Select(albumTrack => new AlbumTrackDto(albumTrack));
    }

}

public class AlbumTrackDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
    [JsonProperty("disc")] public int? Disc { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("quality")] public int? Quality { get; set; }
    [JsonProperty("track")] public int? Track { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }
    
    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }
    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }

    public AlbumTrackDto(AlbumTrack albumTrack)
    {
        ColorPalette = albumTrack.Album.ColorPalette;
        Cover = albumTrack.Album.Cover;
        Date = albumTrack.Track.Date;
        Disc = albumTrack.Track.DiscNumber;
        Duration = albumTrack.Track.Duration;
        Favorite = albumTrack.Track.TrackUser.Count != 0;
        Filename = albumTrack.Track.Filename;
        Folder = albumTrack.Track.Folder;
        FolderId = albumTrack.Track.FolderId;
        Id = albumTrack.Track.Id;
        LibraryId = albumTrack.Album.LibraryId;
        Name = albumTrack.Track.Name;
        Origin = SystemInfo.DeviceId;
        Path = albumTrack.Track.Folder + "/" + albumTrack.Track.Filename;
        Quality = albumTrack.Track.Quality;
        Track = albumTrack.Track.TrackNumber;
        Lyrics = albumTrack.Track.Lyrics;
        Type = "tracks";
        
        var artists = Databases.MediaContext.ArtistTrack
            .Where(at => at.TrackId == albumTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();
        
        Artist = artists
            .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(artistTrack => new ArtistDto(artistTrack));
        
        Album = albumTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(trackAlbum => new AlbumDto(trackAlbum));
    }
}

public class AlbumDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }

    public AlbumDto(AlbumArtist albumArtist)
    {
        ColorPalette = albumArtist.Artist.ColorPalette;
        Cover = albumArtist.Artist.Cover;
        Description = albumArtist.Artist.Description;
        Folder = albumArtist.Artist.Folder;
        Id = albumArtist.Artist.Id;
        LibraryId = albumArtist.Artist.LibraryId;
        Name = albumArtist.Artist.Name;
        Origin = SystemInfo.DeviceId;
    }

    public AlbumDto(AlbumTrack albumTrack)
    {
        ColorPalette = albumTrack.Album.ColorPalette;
        Cover = albumTrack.Album.Cover;
        Description = albumTrack.Album.Description;
        Folder = albumTrack.Album.Folder;
        Id = albumTrack.Album.Id;
        LibraryId = albumTrack.Album.LibraryId;
        Name = albumTrack.Album.Name;
        Origin = SystemInfo.DeviceId;
    }
}