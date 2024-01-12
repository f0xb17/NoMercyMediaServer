using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class Credits
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("cast")] public List<Cast> Cast { get; set; } = new();

    [JsonProperty("crew")] public List<Crew> Crew { get; set; } = new();

    [JsonProperty("guest_stars")] public List<GuestStar> GuestStars { get; set; } = new();
}