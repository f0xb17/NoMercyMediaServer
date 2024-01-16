using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionImages
{

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("backdrops")] public Backdrop[] Backdrops { get; set; } = [];

    [JsonProperty("posters")] public Poster[] Posters { get; set; } = [];
}