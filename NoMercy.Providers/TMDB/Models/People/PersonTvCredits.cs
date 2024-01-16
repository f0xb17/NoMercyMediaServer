using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonTvCredits
{
    [JsonProperty("cast")] public Cast[] Cast { get; set; } = [];

    [JsonProperty("crew")] public Crew[] Crew { get; set; } = [];
}