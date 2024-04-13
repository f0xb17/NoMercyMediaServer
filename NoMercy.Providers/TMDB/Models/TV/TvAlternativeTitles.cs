using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvAlternativeTitles
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("results")] public TvAlternativeTitle[] Results { get; set; } = [];
}

public class TvAlternativeTitle
{
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
}