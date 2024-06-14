using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public record CarouselResponseDto<T>
{
    [JsonProperty("data")] public IEnumerable<T> Data { get; set; } = [];
}

public record CarouselResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("track_id")] public string? TrackId { get; set; }
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("tracks")] public int Tracks { get; set; }

    public CarouselResponseItemDto(Artist artist)
    {
        ColorPalette = artist.ColorPalette;
        Cover = artist.Cover;
        Disambiguation = artist.Disambiguation;
        Description = artist.Description;
        Folder = artist.Folder ?? "";
        Id = artist.Id.ToString();
        LibraryId = artist.LibraryId ?? Ulid.Empty;
        Name = artist.Name;
        Type = "artists";

        Tracks = artist.ArtistTrack
            .Where(artistTrack => artistTrack.Track.Duration != null)
            .DistinctBy(artistTrack => artistTrack.Track.Name.ToLower())
            .Count();
    }

    public CarouselResponseItemDto(Album album)
    {
        ColorPalette = album.ColorPalette;
        Cover = album.Cover;
        Disambiguation = album.Disambiguation;
        Description = album.Description;
        Folder = album.Folder ?? "";
        Id = album.Id.ToString();
        LibraryId = album.LibraryId ?? Ulid.Empty;
        Name = album.Name;
        Type = "albums";

        Tracks = album.AlbumTrack
            .Where(albumTrack => albumTrack.Track.Duration != null)
            .DistinctBy(albumTrack => albumTrack.Track.Name.ToLower())
            .Count();
    }

    public CarouselResponseItemDto(ArtistUser playlist)
    {
        ColorPalette = playlist.Artist.ColorPalette;
        Cover = playlist.Artist.Cover;
        Disambiguation = playlist.Artist.Disambiguation;
        Description = playlist.Artist.Description;
        Folder = playlist.Artist.Folder ?? "";
        Id = playlist.Artist.Id.ToString();
        LibraryId = playlist.Artist.LibraryId ?? Ulid.Empty;
        Name = playlist.Artist.Name;
        Type = "artists";

        Tracks = playlist.Artist.ArtistTrack
            .Where(artistTrack => artistTrack.Track.Duration != null)
            .DistinctBy(artistTrack => artistTrack.Track.Name.ToLower())
            .Count();
    }

    public CarouselResponseItemDto(AlbumUser playlist)
    {
        ColorPalette = playlist.Album.ColorPalette;
        Cover = playlist.Album.Cover;
        Disambiguation = playlist.Album.Disambiguation;
        Description = playlist.Album.Description;
        Folder = playlist.Album.Folder ?? "";
        Id = playlist.Album.Id.ToString();
        LibraryId = playlist.Album.LibraryId ?? Ulid.Empty;
        Name = playlist.Album.Name;
        Type = "albums";

        Tracks = playlist.Album.AlbumTrack
            .Where(albumTrack => albumTrack.Track.Duration != null)
            .DistinctBy(albumTrack => albumTrack.Track.Name.ToLower())
            .Count();
    }

    public CarouselResponseItemDto(Playlist playlist)
    {
        ColorPalette = playlist.ColorPalette;
        Cover = playlist.Cover;
        Description = playlist.Description;
        Id = playlist.Id.ToString();
        Name = playlist.Name;
        Type = "playlists";

        Tracks = playlist.Tracks
            .Where(playlistTrack => playlistTrack.Track.Duration != null)
            .DistinctBy(playlistTrack => playlistTrack.Track.Name.ToLower())
            .Count();
    }
}

public record TopMusicDto<T>
{
    [JsonProperty("id")] public string Id { get; set; } = "";
    [JsonProperty("name")] public string Name { get; set; } = "";
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = "albums";
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("tracks")] public int Tracks { get; set; }
    [JsonProperty("items")] public IEnumerable<T> Items { get; set; } = [];

}