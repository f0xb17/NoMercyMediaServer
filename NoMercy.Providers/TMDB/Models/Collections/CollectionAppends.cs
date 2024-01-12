using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionAppends(Ci images, CombinedTranslations translations)
    : CollectionDetails(0, "", "", "", "", Array.Empty<Movies.Movie>())
{
    [JsonProperty("images")] public Ci Images { get; set; } = images;

    [JsonProperty("translations")] public CombinedTranslations Translations { get; set; } = translations;
}

public class Ci(Backdrop[] backdrops, Poster[] posters)
{
    [JsonProperty("backdrops")] public List<Backdrop> Backdrops { get; set; } = new();

    [JsonProperty("posters")] public List<Poster> Posters { get; set; } = new();
}