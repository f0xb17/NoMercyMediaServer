using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.Shared;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieAppends : MovieDetails
{
    [JsonProperty("alternative_titles")] public MovieAlternativeTitles AlternativeTitles { get; set; } = new();
    
    [JsonProperty("credits")] public MovieCredits Credits { get; set; }

    [JsonProperty("external_ids")] public MovieExternalIds? ExternalIds { get; set; }

    [JsonProperty("images")] public MovieImages Images { get; set; }

    [JsonProperty("keywords")] public MovieKeywords? Keywords { get; set; }

    [JsonProperty("recommendations")] public MovieRecommendations? Recommendations { get; set; }

    [JsonProperty("similar")] public MovieSimilar? Similar { get; set; }

    [JsonProperty("translations")] public CombinedTranslations Translations { get; set; } = new();

    [JsonProperty("videos")] public MovieVideos Videos { get; set; }

    [JsonProperty("watch/providers")] public MovieWatchProviders? WatchProviders { get; set; }

    [JsonProperty("genres")] public new Genre[] Genres { get; set; } = [];

    [JsonProperty("release_dates")] public MovieReleaseDates? ReleaseDates { get; set; }

}