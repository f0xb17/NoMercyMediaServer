using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class VideoDto
{
    [JsonProperty("src")] public string? Src { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("site")] public string? Site { get; set; }

    [JsonProperty("size")] public int Size { get; set; }

    public VideoDto(Database.Models.Media media)
    {
        Src = media.Src;
        Type = media.Type;
        Name = media.Name;
        Site = media.Site;
        Size = media.Size;
    }
}

public class GenreDto
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("item_id")] public int? ItemId { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    public GenreDto(GenreMovie genreMovie)
    {
        Id = genreMovie.GenreId;
        Name = genreMovie.Genre.Name;
    }

    public GenreDto(GenreTv genreTv)
    {
        Id = genreTv.GenreId;
        Name = genreTv.Genre.Name;
    }

    public GenreDto(Genre genreMovie)
    {
        Id = genreMovie.Id;
        Name = genreMovie.Name;
    }
}

public class RatingDto
{
    [JsonProperty("rating")] public string RatingRating { get; set; }

    [JsonProperty("iso31661")] public string Iso31661 { get; set; }
}

public class ColorPalettesDto
{
    [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
    public IColorPalettes Logo { get; set; }

    [JsonProperty("poster", NullValueHandling = NullValueHandling.Ignore)]
    public IColorPalettes Poster { get; set; }

    [JsonProperty("backdrop", NullValueHandling = NullValueHandling.Ignore)]
    public IColorPalettes Backdrop { get; set; }
}

public class StatusResponseDto<T>
{
    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public T? Data { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
    public dynamic[]? Args { get; set; }
}

public class AvailableResponseDto
{
    [JsonProperty("available")] public bool Available { get; set; }
    [JsonProperty("server")] public string? Message { get; set; }
}