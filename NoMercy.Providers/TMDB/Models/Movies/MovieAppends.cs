#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieAppends : MovieDetails
{
    [JsonProperty("alternative_titles")] public MovieAlternativeTitles AlternativeTitles { get; set; } = new();    
    [JsonProperty("credits")] public MovieCredits Credits { get; set; } = new();
    [JsonProperty("external_ids")] public MovieExternalIds ExternalIds { get; set; } = new();
    [JsonProperty("images")] public MovieImages Images { get; set; } = new();
    [JsonProperty("keywords")] public MovieKeywords Keywords { get; set; } = new();
    [JsonProperty("recommendations")] public MovieRecommendations Recommendations { get; set; } = new();
    [JsonProperty("similar")] public MovieSimilar Similar { get; set; } = new();
    [JsonProperty("translations")] public CombinedTranslations Translations { get; set; } = new();
    [JsonProperty("videos")] public MovieVideos Videos { get; set; } = new();
    [JsonProperty("watch/providers")] public MovieWatchProviders WatchProviders { get; set; }
    [JsonProperty("genres")] public new Genre[] Genres { get; set; } = [];
    [JsonProperty("release_dates")] public MovieReleaseDates ReleaseDates { get; set; } = new();
}