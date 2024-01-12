using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new List<TvTranslation> Translations { get; set; }
}

public class TvTranslation : SharedTranslation
{
    [JsonProperty("data")] public new TvTranslationData Data { get; set; }
}

public class TvTranslationData : SharedTranslationData
{
    [JsonProperty("name")] public string Name { get; set; }
}