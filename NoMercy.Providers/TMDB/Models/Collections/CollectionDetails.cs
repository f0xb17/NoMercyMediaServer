using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionDetails(
    int id,
    string name,
    string posterPath,
    string backdropPath,
    string overview,
    Movies.Movie[] parts)
    : Collection(id, name, posterPath, backdropPath)
{
    [JsonProperty("overview")] public string Overview { get; set; } = overview;

    [JsonProperty("parts")] public List<Movies.Movie> Parts { get; set; } = new();
}