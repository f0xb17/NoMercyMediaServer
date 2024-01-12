using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Networks;

public class NetworkDetails : Network
{
    [JsonProperty("headquarters")] public string Headquarters { get; set; } = string.Empty;

    [JsonProperty("homepage")] public Uri? Homepage { get; set; }
}