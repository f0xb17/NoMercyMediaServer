using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Season;

public class Images
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("posters")] public Poster[] Posters { get; set; } = [];
}