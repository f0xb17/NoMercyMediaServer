using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvImages
{
    [JsonProperty("backdrops")] public List<Backdrop> Backdrops { get; set; }

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("posters")] public List<Poster> Posters { get; set; }
}