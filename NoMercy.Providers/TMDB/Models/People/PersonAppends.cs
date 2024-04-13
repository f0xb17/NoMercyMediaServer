#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonAppends : PersonDetails
{
    [JsonProperty("movie_credits")] public PersonCredits MovieCredits { get; set; } = new();
    
    [JsonProperty("credits")] public PersonCredits Credits { get; set; } = new();
    
    [JsonProperty("combined_credits")] public PersonCredits CombinedCredits { get; set; } = new();

    [JsonProperty("tv_credits")] public PersonCredits TvCredits { get; set; } = new();
    
    [JsonProperty("images")] public PersonImages Images { get; set; } = new();

    [JsonProperty("translations")] public PersonTranslations Translations { get; set; } = new();
}