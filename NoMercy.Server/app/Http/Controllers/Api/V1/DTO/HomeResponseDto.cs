using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class HomeResponseDto
{
    [JsonProperty("data")] public IEnumerable<GenreRowDto> Data { get; set; } = [];
}

public class GenreRowDto
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("moreLink")] public string MoreLink { get; set; }

    [JsonProperty("items")] public IEnumerable<GenreRowItemDto> Items { get; set; } = [];
}

public class GenreRowItemDto
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
    public string? Logo { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("year")] public int? Year { get; set; }

    [JsonProperty("mediaType")] public string? MediaType { get; set; }

    [JsonProperty("colorPalette", NullValueHandling = NullValueHandling.Ignore)] 
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }

    [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
    public RatingClass? Rating { get; set; }

    [JsonProperty("videos")] public VideoDto[]? Videos { get; set; }
    
    
    public GenreRowItemDto(GenreMovie genreMovie)
    {
        Id = genreMovie.Movie.Id;
        Title = genreMovie.Movie.Title;
        Overview = genreMovie.Movie.Overview;
        Poster = genreMovie.Movie.Poster;
        Backdrop = genreMovie.Movie.Backdrop;
        TitleSort = genreMovie.Movie.Title.TitleSort(genreMovie.Movie.ReleaseDate);
        Year = genreMovie.Movie.ReleaseDate.ParseYear();
        MediaType = "movie";
        ColorPalette = genreMovie.Movie.ColorPalette;
    }

    public GenreRowItemDto(GenreTv genreTv)
    {
        Id = genreTv.Tv.Id;
        Title = genreTv.Tv.Title;
        Overview = genreTv.Tv.Overview;
        Poster = genreTv.Tv.Poster;
        Backdrop = genreTv.Tv.Backdrop;
        TitleSort = genreTv.Tv.Title.TitleSort(genreTv.Tv.FirstAirDate);
        Type = genreTv.Tv.Type;
        Year = genreTv.Tv.FirstAirDate.ParseYear();
        MediaType = "tv";
        ColorPalette = genreTv.Tv.ColorPalette;
        
    }
}

public class RatingClass
{
    [JsonProperty("rating")] public string Rating { get; set; }

    [JsonProperty("meaning")] public string Meaning { get; set; }

    [JsonProperty("order")] public long Order { get; set; }

    [JsonProperty("iso31661")] public string Iso31661 { get; set; }

    [JsonProperty("image")] public string Image { get; set; }
}