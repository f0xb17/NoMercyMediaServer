using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;

public record AlbumResponseDto
{
    [JsonProperty("data")] public AlbumResponseItemDto? Data { get; set; }

    public static readonly Func<MediaContext, Guid, Guid, Task<Album?>> GetAlbum =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Guid id) => mediaContext.Albums
            .AsNoTracking()
            .Where(album => album.Id == id)
            
            .Where(album => album.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            .Include(album => album.Library)
            
            .Include(album => album.AlbumUser
                .Where(albumUser => albumUser.UserId == userId)
            )
            .Include(album => album.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Artist)
                    .ThenInclude(artist => artist.Translations)
                
            
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
                            .ThenInclude(artist => artist.Translations)
            
                .Include(album => album.AlbumArtist)
                    .ThenInclude(albumArtist => albumArtist.Artist)
                        .ThenInclude(artist => artist.Images)
            
            .Include(album => album.Images)
            .Include(album => album.Translations)

            .Include(album => album.AlbumMusicGenre)
                .ThenInclude(artistMusicGenre => artistMusicGenre.MusicGenre)
            
            .FirstOrDefault());
}

public record AlbumResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("country")] public string? Country { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
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
    [JsonProperty("images")] public IEnumerable<ImageDto> Images { get; set; }
    [JsonProperty("genres")] public IEnumerable<GenreDto> Genres { get; set; }

    public AlbumResponseItemDto(Album album, string? country = "US")
    {
        ColorPalette = album.ColorPalette;
        Cover = album.Cover;
        Disambiguation = album.Disambiguation;
        Description = album.Description;
        Favorite = album.AlbumUser.Count != 0;
        Folder = album.Folder;
        Id = album.Id;
        LibraryId = album.LibraryId;
        Name = album.Name;
        Type = "albums";

        using MediaContext context = new();
        var artists = context.AlbumTrack
            .AsNoTracking()
            .Where(at => at.TrackId == album.Id)
            .Include(at => at.Track)
                .ThenInclude(track => track.ArtistTrack)
                    .ThenInclude(artistTrack => artistTrack.Artist)
                        .ThenInclude(artist => artist.Translations)
            .ToList() ?? [];

        Artists = artists
            .SelectMany(albumTrack => albumTrack.Track.ArtistTrack)
            .Select(albumTrack => new ArtistDto(albumTrack, country!));
        
        // Artists = album.AlbumArtist
        //     .DistinctBy(trackArtist => trackArtist.ArtistId)
        //     .Select(albumArtist => new ArtistDto(albumArtist, country!));

        Genres = album.AlbumMusicGenre.Select(musicGenre => new GenreDto(musicGenre));
        
        Images = album.Images.Select(image => new ImageDto(image));
        
        Tracks = album.AlbumTrack
            .Select(albumTrack => new AlbumTrackDto(albumTrack, country!));

    }
}

public record AlbumTrackDto
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

    public AlbumTrackDto(AlbumTrack albumTrack, string country)
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

        using MediaContext context = new();
        var artists = context.ArtistTrack
            .Where(at => at.TrackId == albumTrack.TrackId)
            .Include(at => at.Artist)
            .ToList();

        Artist = artists
            // .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(artistTrack => new ArtistDto(artistTrack, country));

        Album = albumTrack.Track.AlbumTrack
            // .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(trackAlbum => new AlbumDto(trackAlbum, country));
    }
}

public record AlbumDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("tracks")] public int Tracks { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("album_artist")] public Guid? AlbumArtist { get; set; }

    public AlbumDto(AlbumArtist albumArtist, string country)
    {
        var description = albumArtist.Album.Translations?
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumArtist.Album.Description;
        
        ColorPalette = albumArtist.Album.ColorPalette;
        Cover = albumArtist.Album.Cover;
        Disambiguation = albumArtist.Album.Disambiguation;
        Folder = albumArtist.Album.Folder;
        Id = albumArtist.Album.Id;
        LibraryId = albumArtist.Album.LibraryId;
        Name = albumArtist.Album.Name;
        Origin = SystemInfo.DeviceId;
        Type = "albums";
        Tracks = albumArtist.Album.Tracks;
        Year = albumArtist.Album.Year;
        
        var trackCount = albumArtist.Album.Tracks;

        var artistTrackCount = albumArtist.Album.AlbumTrack?
            .Select(albumTrack => albumTrack.Track)
            .SelectMany(track => track.ArtistTrack)
            .Count();
        
        var isAlbumArtist = artistTrackCount >= trackCount * 0.45;
        
        AlbumArtist = isAlbumArtist
                ? albumArtist.ArtistId 
                : null;
    }

    public AlbumDto(AlbumTrack albumTrack, string country)
    {
        
        var description = albumTrack.Album.Translations?
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumTrack.Album.Description;
        
        ColorPalette = albumTrack.Album.ColorPalette;
        Cover = albumTrack.Album.Cover;
        Disambiguation = albumTrack.Album.Disambiguation;
        Folder = albumTrack.Album.Folder;
        Id = albumTrack.Album.Id;
        LibraryId = albumTrack.Album.LibraryId;
        Name = albumTrack.Album.Name;
        Tracks = albumTrack.Album.Tracks;
        Year = albumTrack.Album.Year;
        Origin = SystemInfo.DeviceId;
        Type = "albums";
        
        AlbumArtist = albumTrack.Album.AlbumArtist?.Count == 1 
            ? albumTrack.Album.AlbumArtist?.FirstOrDefault()?.ArtistId 
            : null;
    }

    public AlbumDto(Album album, string country)
    {
        var description = album.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : album.Description;
        
        ColorPalette = album.ColorPalette;
        Cover = album.Cover;
        Disambiguation = album.Disambiguation;
        Description = album.Description;
        Folder = album.Folder;
        Id = album.Id;
        LibraryId = album.LibraryId;
        Name = album.Name;
        Tracks = album.Tracks;
        Year = album.Year;
        Origin = SystemInfo.DeviceId;
        Type = "albums";
        
        AlbumArtist = album.AlbumArtist?.Count == 1 
            ? album.AlbumArtist?.FirstOrDefault()?.ArtistId 
            : null;
    }
}