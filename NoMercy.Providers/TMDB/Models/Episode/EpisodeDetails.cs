using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class EpisodeDetails : Episode
{
    [JsonProperty("crew")] public Crew[] Crew { get; set; } = [];
    [JsonProperty("cast")] public Cast[] Cast { get; set; } = [];
    [JsonProperty("guest_stars")] public GuestStar[] GuestStars { get; set; } = [];
}