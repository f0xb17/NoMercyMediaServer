using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonCombinedCredits
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("cast")] public Cast[] Cast { get; set; } = [];

    [JsonProperty("crew")] public Cast[] Crew { get; set; } = [];
}