using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonDetails : Person
{
    [JsonProperty("deathday")] public DateTime? DeathDay { get; set; }
    [JsonProperty("homepage")] public Uri? Homepage { get; set; }
}