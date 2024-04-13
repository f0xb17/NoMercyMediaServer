#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Web.Http.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

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

    public VideoDto(TvVideo media)
    {
        Src = media.Key;
        Type = "video";
        Name = media.Name;
        Site = media.Site;
        Size = media.Size;
    }

    public VideoDto(MovieVideo media)
    {
        Src = media.Key;
        Type = "video";
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

    public GenreDto(Providers.TMDB.Models.Shared.Genre genreMovie)
    {
        Id = genreMovie.Id;
        Name = genreMovie.Name;
    }
}

public class RatingDto
{
    [JsonProperty("rating")] public string RatingRating { get; set; }

    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
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
    public T Data { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
    public dynamic[]? Args { get; set; } = [];
}

public class AvailableResponseDto
{
    [JsonProperty("available")] public bool Available { get; set; }
    [JsonProperty("server")] public string? Message { get; set; }
}

public class LikeRequestDto
{
    [JsonProperty("value")] public bool Value { get; set; }
}

public class PeopleDto
{
    [JsonProperty("character", NullValueHandling = NullValueHandling.Ignore)]
    public string? Character { get; set; }

    [JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
    public string? Job { get; set; }

    [JsonProperty("profilePath")] public string? ProfilePath { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("knownForDepartment")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("deathDay")] public DateTime? DeathDay { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }

    [JsonProperty("color_palette", NullValueHandling = NullValueHandling.Include)]
    public IColorPalettes? ColorPalette { get; set; }

    public PeopleDto(Cast cast)
    {
        Character = cast.Role.Character;
        ProfilePath = cast.Person.Profile;
        KnownForDepartment = cast.Person.KnownForDepartment;
        Name = cast.Person.Name;
        ColorPalette = cast.Person.ColorPalette;
        DeathDay = cast.Person.DeathDay;
        Gender = cast.Person.Gender;
        Order = cast.Role.Order;
        Id = cast.Person.Id;
    }

    public PeopleDto(Providers.TMDB.Models.Shared.Cast cast)
    {
        Character = cast.Character;
        ProfilePath = cast.ProfilePath;
        KnownForDepartment = cast.KnownForDepartment;
        Name = cast.Name ?? string.Empty;
        ColorPalette = new IColorPalettes();
        Gender = Enum.Parse<Gender>(cast.Gender.ToString(), true).ToString();
        Order = cast.Order;
        Id = cast.Id;
    }

    public PeopleDto(Crew crew)
    {
        Job = crew.Job.Task;
        ProfilePath = crew.Person.Profile;
        KnownForDepartment = crew.Person.KnownForDepartment;
        Name = crew.Person.Name;
        ColorPalette = crew.Person.ColorPalette;
        DeathDay = crew.Person.DeathDay;
        Gender = crew.Person.Gender;
        Id = crew.Person.Id;
        Order = crew.Job.Order;
    }
    
    public PeopleDto(Providers.TMDB.Models.Shared.Crew crew)
    {
        Job = crew.Job;
        ProfilePath = crew.ProfilePath;
        KnownForDepartment = crew.KnownForDepartment;
        Name = crew.Name;
        ColorPalette = new IColorPalettes();
        Gender = Enum.Parse<Gender>(crew.Gender.ToString(), true).ToString();
        Id = crew.Id;
        Order = crew.Order;
    }
}

public class ContentRating
{
    [JsonProperty("rating")] public string? Rating { get; set; }
    [JsonProperty("iso_3166_1")] public string? Iso31661 { get; set; }
}

public class DataResponseDto<T>
{
    [JsonProperty("data")] public T? Data { get; set; } = default(T);
}