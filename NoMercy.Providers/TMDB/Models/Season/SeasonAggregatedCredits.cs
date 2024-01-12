using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Season;

public class SeasonAggregatedCredits
{
    [JsonProperty("id")] public int Id { get; set; }
    
    [JsonProperty("cast")] public List<AggregatedCast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<AggregatedCrew> Crew { get; set; } = new();
}