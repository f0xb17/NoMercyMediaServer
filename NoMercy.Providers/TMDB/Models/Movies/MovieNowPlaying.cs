using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieNowPlaying : PaginatedResponse<Movie>
{
    [JsonProperty("dates")] public Dates Dates { get; set; } = new();
}

public class Dates
{
    [JsonProperty("maximum")] public DateTime? Maximum { get; set; }

    [JsonProperty("minimum")] public DateTime? Minimum { get; set; }
}