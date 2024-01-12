using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Shared;

public class PaginatedResponse<T>
{
    [JsonProperty("page")] public int Page { get; set; }

    [JsonProperty("results")] public List<T> Results { get; set; } = new();

    [JsonProperty("total_pages")] public int TotalPages { get; set; }

    [JsonProperty("total_results")] public int TotalResults { get; set; }
}