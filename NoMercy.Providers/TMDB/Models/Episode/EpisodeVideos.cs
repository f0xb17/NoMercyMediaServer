using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class Videos
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public Video[] Results { get; set; } = [];
}