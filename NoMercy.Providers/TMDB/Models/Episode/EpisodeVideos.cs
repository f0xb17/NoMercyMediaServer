using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class Videos
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public List<Video> Results { get; set; } = new();
}