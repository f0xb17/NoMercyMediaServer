using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new TvTranslation[] Translations { get; set; } = [];
}

public class TvTranslation : SharedTranslation
{
    [JsonProperty("data")] public new TvTranslationData Data { get; set; }
}

public class TvTranslationData : SharedTranslationData
{
    [JsonProperty("name")] public string Name { get; set; }
}