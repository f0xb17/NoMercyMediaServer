using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieVideos
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public Video[] Results { get; set; } = [];
}