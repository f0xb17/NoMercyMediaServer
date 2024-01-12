using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieTranslations : SharedTranslations
{
    [JsonProperty("translations")] public new List<MovieTranslation> Translations { get; set; } = new();
}

public class MovieTranslation : SharedTranslation
{
    [JsonProperty("data")] public new MovieTranslationData Data { get; set; } = new();
}

public class MovieTranslationData : SharedTranslationData
{
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
}