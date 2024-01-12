using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class EpisodeDetails : Episode
{
    [JsonProperty("crew")] public List<Crew> Crew { get; set; } = new();

    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("guest_stars")] public List<GuestStar> GuestStars { get; set; } = new();
}