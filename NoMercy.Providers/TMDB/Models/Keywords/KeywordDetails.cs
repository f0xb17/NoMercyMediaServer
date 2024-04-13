using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Keywords;

public class KeywordDetails
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}