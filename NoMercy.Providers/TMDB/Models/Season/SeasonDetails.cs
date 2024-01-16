using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Episode;

namespace NoMercy.Providers.TMDB.Models.Season;

public class SeasonDetails : Season
{
    [JsonProperty("episodes")] public EpisodeDetails[] Episodes { get; set; } = [];
}