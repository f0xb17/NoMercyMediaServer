using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieUpcoming : PaginatedResponse<Movie>
{
    [JsonProperty("dates")] public UpcomingDates Dates { get; set; } = new();
}

public class UpcomingDates
{
    [JsonProperty("maximum")] public DateTime? Maximum { get; set; }

    [JsonProperty("minimum")] public DateTime? Minimum { get; set; }
}