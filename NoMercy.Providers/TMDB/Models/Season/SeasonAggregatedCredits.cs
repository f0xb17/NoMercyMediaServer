using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Season;

public class SeasonAggregatedCredits
{
    [JsonProperty("id")] public int Id { get; set; }
    
    [JsonProperty("cast")] public AggregatedCast[] Cast { get; set; } = [];

    [JsonProperty("crew")] public AggregatedCrew[] Crew { get; set; } = [];
}