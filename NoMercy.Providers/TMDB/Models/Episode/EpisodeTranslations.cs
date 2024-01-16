using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class EpisodeTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new EpisodeTranslation[] Translations { get; set; } = [];
}

public class EpisodeTranslation : SharedTranslation
{

    [JsonProperty("data")] public new EpisodeTranslationData Data { get; set; } = new();
}

public class EpisodeTranslationData : SharedTranslationData
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}