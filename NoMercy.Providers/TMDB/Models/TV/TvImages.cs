using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvImages
{
    [JsonProperty("backdrops")] public Backdrop[] Backdrops { get; set; } = [];

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("posters")] public Poster[] Posters { get; set; } = [];
}