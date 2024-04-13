using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieCredits
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("cast")] public Cast[] Cast { get; set; } = [];
    [JsonProperty("crew")] public Crew[] Crew { get; set; } = [];
}