using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonCredits
{
    [JsonProperty("cast")] public PersonCredit[] Cast { get; set; } = [];

    [JsonProperty("crew")] public PersonCredit[] Crew { get; set; } = [];
}