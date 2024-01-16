using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonMovieCredits
{
    [JsonProperty("cast")] public Cast[] Cast { get; set; } = [];

    [JsonProperty("crew")] public Cast[] Crew { get; set; } = [];
}