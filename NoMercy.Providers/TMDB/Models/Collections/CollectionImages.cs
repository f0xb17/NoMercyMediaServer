using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionImages
{

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("backdrops")] public List<Backdrop> Backdrops { get; set; } = new();

    [JsonProperty("posters")] public List<Poster> Posters { get; set; } = new();
}