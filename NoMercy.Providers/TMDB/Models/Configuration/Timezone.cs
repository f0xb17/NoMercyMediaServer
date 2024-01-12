using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Configuration;

public class Timezone
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;
    [JsonProperty("zones")] public string[] Zones { get; set; } = [];
}