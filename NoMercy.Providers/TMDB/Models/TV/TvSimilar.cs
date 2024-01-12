using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvSimilar : PaginatedResponse<SimilarTvShow>
{
}

public class SimilarTvShow : TvShow
{
    [JsonProperty("adult")] public bool Adult { get; set; }
}