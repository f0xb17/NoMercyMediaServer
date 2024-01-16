using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.WatchProviders;

public class MovieProviders
{
    [JsonProperty("results")] public Provider[] Results { get; set; } = [];
}