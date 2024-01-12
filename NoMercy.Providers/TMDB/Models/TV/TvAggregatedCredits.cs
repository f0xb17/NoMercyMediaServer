using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvAggregatedCredits
{
    [JsonProperty("cast")] public List<AggregatedCast> Cast { get; set; }

    [JsonProperty("crew")] public List<AggregatedCrew> Crew { get; set; }

    [JsonProperty("id")] public int Id { get; set; }
}