using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Search;

public class Collections : PaginatedResponse<CollectionResult>
{
}

public class CollectionResult
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("poster_path")] public string? PosterPath { get; set; }
}