using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Networks;

public class NetworkImages
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("logos")] public List<Logo> Logos { get; set; } = new();
}
