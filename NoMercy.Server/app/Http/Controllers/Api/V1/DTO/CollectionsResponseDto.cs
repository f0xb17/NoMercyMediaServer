using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class CollectionsResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IEnumerable<CollectionsResponseItemDto> Data { get; set; }
}

public class CollectionsResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("favorite")] public bool Favorite { get; set; }

    [JsonProperty("watched")] public bool Watched { get; set; }

    [JsonProperty("logo")] public string? Logo { get; set; }

    [JsonProperty("mediaType")] public string MediaType { get; set; }

    [JsonProperty("numberOfEpisodes")] public int? NumberOfEpisodes { get; set; }

    [JsonProperty("haveEpisodes")] public int? HaveEpisodes { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("color_palette")] 
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("year")] public int? Year { get; set; }

    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("videos")] public VideoDto[]? Videos { get; set; } = [];
    
    public CollectionsResponseItemDto(CollectionMovie movie)
    {
        Id = movie.Movie.Id;
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.Movie.ReleaseDate.ParseYear();
        Overview = movie.Movie.Overview;
        ColorPalette = movie.Movie.ColorPalette;
        Poster = movie.Movie.Poster;
        Title = movie.Movie.Title;
        TitleSort = movie.Movie.Title.TitleSort(movie.Movie.ReleaseDate);
        Type = "movie";
        Genres = movie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public CollectionsResponseItemDto(Collection collection)
    {
        Id = collection.Id;
        Backdrop = collection.Backdrop;
        Logo = collection.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        
        MediaType = "collections";
        Year = collection.CollectionMovies?
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate
            .ParseYear();
        
        Overview = collection.Overview;
        ColorPalette = collection.ColorPalette;
        Poster = collection.Poster;
        Title = collection.Title;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies?
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate
                .ParseYear());
        
        Type = "collections";
        NumberOfEpisodes = collection.Parts;
        HaveEpisodes = collection.CollectionMovies?
            .Where(collectionMovie => collectionMovie.Movie.VideoFiles.Any(v => v.Folder != null))
            .Count() ?? 0;
        Genres = collection.CollectionMovies?
            .Select(genreTv => genreTv.Movie)
            .SelectMany(movie => movie.GenreMovies?
                .Select(genreMovie => genreMovie.Genre) ?? Array.Empty<Genre>())
            .Select(genre => new GenreDto(genre))
            .ToArray() ?? [];
        
        VideoId = collection.CollectionMovies?
            .FirstOrDefault()
            ?.Movie.Video;
    }
}
