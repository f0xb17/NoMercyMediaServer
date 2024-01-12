using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionsTranslations(List<CollectionsTranslation> translations) : SharedTranslations
{
    [JsonProperty("translations")] public new List<CollectionsTranslation> Translations { get; set; } = translations;
}

public class CollectionsTranslation : SharedTranslation
{
    [JsonProperty("data")] public new CollectionsTranslationData Data { get; set; } = new();
}

public class CollectionsTranslationData : SharedTranslationData
{
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
}