using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionImages
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("backdrops")] public Image[] Backdrops { get; set; } = [];
    [JsonProperty("posters")] public Image[] Posters { get; set; } = [];
    [JsonProperty("logos")] public Image[] Logos { get; set; } = [];
}