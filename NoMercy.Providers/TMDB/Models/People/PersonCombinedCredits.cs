using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonCombinedCredits
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<Cast> Crew { get; set; } = new();
}