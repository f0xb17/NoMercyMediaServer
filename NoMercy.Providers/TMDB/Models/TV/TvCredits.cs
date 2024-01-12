using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvCredits
{
    [JsonProperty("cast")] public List<Cast> Cast { get; set; }

    [JsonProperty("crew")] public List<Crew> Crew { get; set; }

    [JsonProperty("id")] public int Id { get; set; }
}