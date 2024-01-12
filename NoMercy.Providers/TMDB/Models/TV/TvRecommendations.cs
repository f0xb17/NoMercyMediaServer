using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvRecommendations : PaginatedResponse<RecommendationsTvShow>
{
}

public class RecommendationsTvShow : TvShow
{
    [JsonProperty("adult")] public bool Adult { get; set; }
}