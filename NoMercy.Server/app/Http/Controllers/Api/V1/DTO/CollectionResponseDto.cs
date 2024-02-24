using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class CollectionResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public CollectionResponseItemDto? Data { get; set; }
}

public class CollectionResponseItemDto
{
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("mediaType")] public string MediaType { get; set; }

    [JsonProperty("colorPalette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("collection")] public CollectionMovieDto[] Collection { get; set; }

    [JsonProperty("numberOfEpisodes")] public int? NumberOfEpisodes { get; set; }

    [JsonProperty("haveEpisodes")] public int? HaveEpisodes { get; set; }

    public CollectionResponseItemDto(Collection collection)
    {
        Title = collection.Title;
        Overview = collection.Overview;
        Backdrop = collection.Backdrop;
        Poster = collection.Poster;
        TitleSort = collection.TitleSort;
        Type = "collections";
        MediaType = "collections";
        ColorPalette = collection.ColorPalette;
        NumberOfEpisodes = collection.Parts;
        HaveEpisodes = collection.CollectionMovies.Count;

        Collection = collection.CollectionMovies
            .OrderBy(movie => movie.Movie.ReleaseDate)
            .Select(movie => new CollectionMovieDto(movie.Movie))
            .ToArray();
    }
}

public class CollectionMovieDto
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("favorite")] public bool Favorite { get; set; }

    [JsonProperty("watched")] public bool Watched { get; set; }

    [JsonProperty("logo")] public string? Logo { get; set; }

    [JsonProperty("mediaType")] public string MediaType { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("colorPalette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("year")] public long Year { get; set; }

    [JsonProperty("genres")] public Genre[] Genres { get; set; }

    [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
    public Certification? Rating { get; set; }

    [JsonProperty("videoId", NullValueHandling = NullValueHandling.Ignore)]
    public string? VideoId { get; set; }

    public CollectionMovieDto(Movie movie)
    {
        Id = movie.Id;
        Backdrop = movie.Backdrop;
        // Favorite = movie.Favorite;
        // Watched = movie.Watched;
        Logo = movie.Images?
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;

        MediaType = "movie";
        Overview = movie.Overview;
        ColorPalette = movie.ColorPalette;
        Poster = movie.Poster;
        Title = movie.Title;
        Type = "movie";
        Year = movie.ReleaseDate.ParseYear();
        Genres = movie.GenreMovies?
            .Select(genreMovie => genreMovie.Genre)
            .ToArray() ?? Array.Empty<Genre>();

        Rating = movie.CertificationMovies?
            .Select(certificationMovie => certificationMovie.Certification)
            .FirstOrDefault();

        VideoId = movie.Video;
    }
}