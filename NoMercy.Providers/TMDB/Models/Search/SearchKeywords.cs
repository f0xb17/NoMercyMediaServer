using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Search;

public class Keywords : PaginatedResponse<KeywordResult>
{
}

public class KeywordResult
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}