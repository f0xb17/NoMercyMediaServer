using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class ContinueWatchingDto
{
    [JsonProperty("data")] public IEnumerable<ContinueWatchingItemDto> Data { get; set; }
}

public class ContinueWatchingItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }

    [JsonProperty("mediaType")] public string? MediaType { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }

    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }

    [JsonProperty("color_palette")] 
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("year")] public long? Year { get; set; }

    [JsonProperty("haveEpisodes")] public long? HaveEpisodes { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("logo")] public string? Logo { get; set; }

    [JsonProperty("rating")] public RatingDto? Rating { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("videos")] public VideoDto[]? Videos { get; set; }

    public ContinueWatchingItemDto(UserData item)
    {
        Id = item.Id;
        Type = item.Type;
        UpdatedAt = item.UpdatedAt;
        CreatedAt = item.CreatedAt;

        if (item.Movie is not null)
        {
            ColorPalette = item.Movie.ColorPalette;
            Year = item.Movie.ReleaseDate.ParseYear();
            Poster = item.Movie.Poster;
            Backdrop = item.Movie.Backdrop;
            Title = item.Movie.Title;
            TitleSort = item.Movie.Title.TitleSort(item.Movie.ReleaseDate);
            Overview = item.Movie.Overview;
            MediaType = "movie";
        }
        else if (item.Tv is not null)
        {
            ColorPalette = item.Tv.ColorPalette;
            Year = item.Tv.FirstAirDate.ParseYear();
            Poster = item.Tv.Poster;
            Backdrop = item.Tv.Backdrop;
            Title = item.Tv.Title;
            TitleSort = item.Tv.Title.TitleSort(item.Tv.FirstAirDate);
            HaveEpisodes = item.Tv.HaveEpisodes;
            Overview = item.Tv.Overview;
            Type = item.Tv.Type;
            MediaType = "tv";
        }
        else if (item.Special is not null)
        {
            ColorPalette = item.Special.ColorPalette;
            Poster = item.Special.Poster;
            Backdrop = item.Special.Backdrop;
            Title = item.Special.Title;
            TitleSort = item.Special.Title.TitleSort();
            Overview = item.Special.Overview;
            MediaType = "special";
        }
    }
}