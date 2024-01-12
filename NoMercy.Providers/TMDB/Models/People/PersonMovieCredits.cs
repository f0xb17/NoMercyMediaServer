using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonMovieCredits
{
    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<Cast> Crew { get; set; } = new();
}