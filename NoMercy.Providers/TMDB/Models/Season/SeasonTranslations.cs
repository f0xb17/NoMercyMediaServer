using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Season;

public class SeasonTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new List<SeasonTranslation> Translations { get; set; } = new();
}

public class SeasonTranslation : SharedTranslation
{
    [JsonProperty("data")] public new SeasonTranslationData Data { get; set; } = new();
}

public class SeasonTranslationData : SharedTranslationData
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}