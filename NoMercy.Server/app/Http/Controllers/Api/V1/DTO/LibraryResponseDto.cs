using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class LibraryResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<LibraryResponseItemDto> Data { get; set; }
}

public class LibraryResponseItemDto
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

    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }
    
    public LibraryResponseItemDto(LibraryMovie movie)
    {
        Id = movie.Movie.Id;
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images?
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
            .ToArray() ?? [];
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray() ?? [];
    }

    public LibraryResponseItemDto(LibraryTv tv)
    {
        Id = tv.Tv.Id;
        Backdrop = tv.Tv.Backdrop;
        Logo = tv.Tv.Images?
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "tv";
        Year = tv.Tv.FirstAirDate.ParseYear();
        Overview = tv.Tv.Overview;
        ColorPalette = tv.Tv.ColorPalette;
        Poster = tv.Tv.Poster;
        Title = tv.Tv.Title;
        TitleSort = tv.Tv.Title.TitleSort(tv.Tv.FirstAirDate);
        Type = "tv";
        NumberOfEpisodes = tv.Tv.NumberOfEpisodes;
        
        HaveEpisodes = tv.Tv.Episodes?
                .Where(episode => episode.SeasonNumber > 0)
                .Count(videoFiles => videoFiles.VideoFiles
                        .DistinctBy(videoFile => videoFile.Folder).Any()) ?? 0;
            
        Genres = tv.Tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray() ?? [];
        VideoId = tv.Tv.Trailer;
        Videos = tv.Tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray() ?? [];
    }
}
