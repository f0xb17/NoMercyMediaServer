using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonAppends : PersonDetails
{
    [JsonProperty("movie_credits")] public PersonMovieCredits MovieCredits { get; set; } = new();

    [JsonProperty("tv_credits")] public PersonTvCredits TvCredits { get; set; } = new();

    [JsonProperty("external_ids")] public PersonExternalIds? ExternalIds { get; set; } = new();

    [JsonProperty("images")] public PersonImages Images { get; set; } = new();

    [JsonProperty("translations")] public PersonTranslations Translations { get; set; } = new();
}