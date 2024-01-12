using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieCredits
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<Crew> Crew { get; set; } = new();
}