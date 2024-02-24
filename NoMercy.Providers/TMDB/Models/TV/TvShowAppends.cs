using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvShowAppends : TvShowDetails
{
    [JsonProperty("aggregate_credits")] public TvAggregatedCredits AggregateCredits { get; set; } = new();

    [JsonProperty("alternative_titles")] public TvAlternativeTitles AlternativeTitles { get; set; } = new();

    [JsonProperty("content_ratings")] public TvContentRatings ContentRatings { get; set; } = new();

    [JsonProperty("credits")] public TvCredits Credits { get; set; } = new();

    [JsonProperty("external_ids")] public TvExternalIds ExternalIds { get; set; } = new();

    [JsonProperty("images")] public TvImages Images { get; set; } = new();

    [JsonProperty("keywords")] public TvKeywords Keywords { get; set; } = new();

    [JsonProperty("recommendations")] public TvRecommendations Recommendations { get; set; } = new();

    [JsonProperty("similar")] public TvSimilar Similar { get; set; } = new();

    [JsonProperty("translations")] public CombinedTranslations Translations { get; set; } = new();

    [JsonProperty("videos")] public TvVideos Videos { get; set; } = new();

    [JsonProperty("watch/providers")] public WatchProviders WatchProviders { get; set; } = new();
}