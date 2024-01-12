using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieAlternativeTitles
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("titles")] public List<MovieAlternativeTitle> Results { get; set; } = new();
}

public class MovieAlternativeTitle
{
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;

    [JsonProperty("title")] public string Title { get; set; } = string.Empty;

    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}