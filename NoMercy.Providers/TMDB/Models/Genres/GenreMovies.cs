using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Genres;

public class GenreMovies
{
    [JsonProperty("genres")] public Genre[] Genres { get; set; } = [];
}