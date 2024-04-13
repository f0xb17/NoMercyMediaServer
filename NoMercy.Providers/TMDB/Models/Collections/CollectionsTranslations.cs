#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionsTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new CollectionsTranslation[] Translations { get; set; }
}

public class CollectionsTranslation : SharedTranslation
{
    [JsonProperty("data")] public new CollectionsTranslationData Data { get; set; }
}

public class CollectionsTranslationData : SharedTranslationData
{
    [JsonProperty("title")] public string? Title { get; set; }
}