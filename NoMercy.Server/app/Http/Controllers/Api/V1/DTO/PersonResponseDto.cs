using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Models.People;
using Person = NoMercy.Database.Models.Person;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class PersonResponseDto
{
    [JsonProperty("nextId")] public long NextId { get; set; }
    [JsonProperty("data")] public PersonResponseItemDto? Data { get; set; }
}

public class PersonResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("alsoKnownAs")] public string[]? AlsoKnownAs { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }
    [JsonProperty("birthday")] public DateTime? Birthday { get; set; }
    [JsonProperty("death_day")] public DateTime? DeathDay { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdbId")] public string? ImdbId { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("profile")] public string? Profile { get; set; }
    [JsonProperty("titleSort")] public string TitleSort { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }

    // [JsonProperty("casts")] public PeopleDto[] Casts { get; set; }
    // [JsonProperty("crews")] public PeopleDto[] Crews { get; set; }
    [JsonProperty("profile_path")] public string ProfilePath { get; set; }

    // [JsonProperty("credits")] public Credits Credits { get; set; }
    [JsonProperty("combined_credits")] public Credits CombinedCredits { get; set; }
    // [JsonProperty("movie_credits")] public Credits MovieCredits { get; set; }
    // [JsonProperty("tv_credits")] public Credits TvCredits { get; set; }
    
    [JsonProperty("external_ids")] public PersonExternalIds? ExternalIds { get; set; }
    [JsonProperty("translations")] public TranslationsDto TranslationsDto { get; set; }
    [JsonProperty("knownFor")] public KnownFor[] KnownFor { get; set; }
    [JsonProperty("images")] public Images Images { get; set; }

    public PersonResponseItemDto(Person person)
    {
        string biography = person.Translations
            .FirstOrDefault()?.Biography ?? person.Biography ?? string.Empty;

        Id = person.Id;
        Name = person.Name;
        Biography = biography;
        Adult = person.Adult;
        AlsoKnownAs = person.AlsoKnownAs is null
            ? []
            : JsonConvert.DeserializeObject<string[]>(person.AlsoKnownAs);
        Birthday = person.BirthDay;
        DeathDay = person.DeathDay;
        Homepage = person.Homepage;
        ImdbId = person.ImdbId;
        KnownForDepartment = person.KnownForDepartment;
        PlaceOfBirth = person.PlaceOfBirth;
        Popularity = person.Popularity;
        Profile = person.Profile;
        ColorPalette = person.ColorPalette;
        CreatedAt = person.CreatedAt;
        UpdatedAt = person.UpdatedAt;
        ExternalIds = person.ExternalIds;
        Gender = person.Gender;

        Images = new Images
        {
            Profiles = person.Images.Select(image => new ImageDto(image))
                .ToArray()
        };

        CombinedCredits = new Credits
        {
            Cast = person.Casts
                .Select(cast => new KnownFor(cast))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray(),

            Crew = person.Crews
                .Select(crew => new KnownFor(crew))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray()
        };
        
        KnownFor = person.Casts
            .Select(crew => new KnownFor(crew))
            .Concat(person.Crews
                .Select(crew => new KnownFor(crew)))
            .OrderByDescending(knownFor => knownFor.Popularity)
            .ToArray();
    }
}

public class Credits
{
    [JsonProperty("cast")] public KnownFor[] Cast { get; set; }

    [JsonProperty("crew")] public KnownFor[] Crew { get; set; }
}

public class KnownFor
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonProperty("genre_ids")] public int[]? GenreIds { get; set; }
    [JsonProperty("id")] public int? Id { get; set; }
    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("poster_path")] public string? PosterPath { get; set; }
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("video")] public bool? Video { get; set; }
    [JsonProperty("vote_average")] public double VoteAverage { get; set; }
    [JsonProperty("vote_count")] public long VoteCount { get; set; }
    [JsonProperty("character")] public string? Character { get; set; }
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("order")] public long? Order { get; set; }
    [JsonProperty("media_type")] public string? MediaType { get; set; }
    [JsonProperty("hasItem")] public bool? HasItem { get; set; }
    [JsonProperty("poster")] public string Poster { get; set; }
    [JsonProperty("year")] public long? Year { get; set; }
    [JsonProperty("origin_country")] public string[] OriginCountry { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }
    [JsonProperty("first_air_date")] public DateTimeOffset? FirstAirDate { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("episode_count")] public long? EpisodeCount { get; set; }
    [JsonProperty("department")] public string? Department { get; set; }
    [JsonProperty("job")] public string? Job { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    
    public KnownFor(Cast cast)
    {
        Character = cast.Role?.Character;
        PosterPath = cast.Movie?.Poster ?? cast.Tv?.Poster;
        Title = cast.Movie?.Title ?? cast.Tv?.Title;
        MediaType = cast.Movie is not null ? "movie" : "tv";
        Year = cast.Movie?.ReleaseDate.ParseYear() ?? cast.Tv?.FirstAirDate.ParseYear();
        Id = cast.Movie?.Id ?? cast.Tv?.Id;
        HasItem = cast.Movie?.VideoFiles.Count != 0 || cast.Tv?.Episodes.Any(e => e.VideoFiles.Count > 0) != false;
        Adult = cast.Movie?.Adult ?? false;
        BackdropPath = cast.Movie?.Backdrop ?? cast.Tv?.Backdrop;
        OriginalLanguage = cast.Movie?.OriginalLanguage ?? cast.Tv?.OriginalLanguage ?? string.Empty;
        Overview = cast.Movie?.Overview ?? cast.Tv?.Overview ?? string.Empty;
        Popularity = cast.Movie?.Popularity ?? cast.Tv?.Popularity ?? 0;
        Poster = cast.Movie?.Poster ?? cast.Tv?.Poster ?? string.Empty;
        ReleaseDate = cast.Movie?.ReleaseDate ?? cast.Tv?.FirstAirDate;
        VoteAverage = cast.Movie?.VoteAverage ?? cast.Tv?.VoteAverage ?? 0;
        VoteCount = cast.Movie?.VoteCount ?? cast.Tv?.VoteCount ?? 0;
        Character = cast.Role?.Character;
        
    }

    public KnownFor(Crew crew)
    {
        Job = crew.Job?.Task;
        PosterPath = crew.Movie?.Poster ?? crew.Tv?.Poster;
        Title = crew.Movie?.Title ?? crew.Tv?.Title;
        MediaType = crew.Movie is not null ? "movie" : "tv";
        Year = crew.Movie?.ReleaseDate.ParseYear() ?? crew.Tv?.FirstAirDate.ParseYear();
        Id = crew.Movie?.Id ?? crew.Tv?.Id;
        HasItem = crew.Movie?.VideoFiles.Count != 0 || crew.Tv?.Episodes.Any(e => e.VideoFiles.Count > 0) != false;
        Adult = crew.Movie?.Adult ?? false;
        BackdropPath = crew.Movie?.Backdrop ?? crew.Tv?.Backdrop;
        OriginalLanguage = crew.Movie?.OriginalLanguage ?? crew.Tv?.OriginalLanguage ?? string.Empty;
        Overview = crew.Movie?.Overview ?? crew.Tv?.Overview ?? string.Empty;
        Popularity = crew.Movie?.Popularity ?? crew.Tv?.Popularity ?? 0;
        Poster = crew.Movie?.Poster ?? crew.Tv?.Poster ?? string.Empty;
        ReleaseDate = crew.Movie?.ReleaseDate ?? crew.Tv?.FirstAirDate;
        VoteAverage = crew.Movie?.VoteAverage ?? crew.Tv?.VoteAverage ?? 0;
        VoteCount = crew.Movie?.VoteCount ?? crew.Tv?.VoteCount ?? 0;
        Job = crew.Job?.Task;
    }
}

public class Images
{
    [JsonProperty("profiles")] public ImageDto[] Profiles { get; set; }
}

public class Profile
{
    [JsonProperty("aspect_ratio")] public double AspectRatio { get; set; }
    [JsonProperty("height")] public long Height { get; set; }
    [JsonProperty("iso_639_1")] public object Iso6391 { get; set; }
    [JsonProperty("file_path")] public string FilePath { get; set; }
    [JsonProperty("vote_average")] public double VoteAverage { get; set; }
    [JsonProperty("vote_count")] public long VoteCount { get; set; }
    [JsonProperty("width")] public long Width { get; set; }
}

public class TranslationsDto
{
    [JsonProperty("translations")] public TranslationDto[] TranslationsTranslations { get; set; }
}

public class TranslationDto
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("english_name")] public string EnglishName { get; set; }
    [JsonProperty("biography")] public string Biography { get; set; }

    public TranslationDto(Translation translation)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name;
        EnglishName = translation.EnglishName;
        Biography = translation.Biography ?? string.Empty;
    }
}