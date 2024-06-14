#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class TmdbPersonTranslations
{
    [JsonProperty("translations")] public TmdbPersonTranslation[] Translations { get; set; } = [];
}

public class TmdbPersonTranslation
{
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("data")] public TmdbPersonTranslationData TmdbPersonTranslationData { get; set; } = new();

    [JsonProperty("english_name")] public string EnglishName { get; set; }
}

public class TmdbPersonTranslationData
{
    [JsonProperty("biography")] public string? Overview { get; set; }
}