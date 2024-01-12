using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieReleaseDates
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public List<Result> Results { get; set; } = new();
}

public class Result
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;

    [JsonProperty("release_dates")] public List<ReleaseDate> ReleaseDates { get; set; } = new();
}

public class ReleaseDate
{
    [JsonProperty("certification")] public string Certification { get; set; } = string.Empty;

    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; } = string.Empty;

    [JsonProperty("release_date")] public DateTime ReleaseDateReleaseDate { get; set; } = DateTime.MinValue;

    [JsonProperty("type")] public int Type { get; set; }

    [JsonProperty("note")] public string? Note { get; set; }
}