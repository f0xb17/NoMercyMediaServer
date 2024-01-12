using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonTvCredits
{
    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<Crew> Crew { get; set; } = new();
}