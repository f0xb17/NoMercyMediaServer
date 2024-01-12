using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class Images
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("stills")] public List<Still> Stills { get; set; } = new();
}