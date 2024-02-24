using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieVideos
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public MovieVideo[] Results { get; set; } = [];
}

public class MovieVideo
{
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;
    
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; } = string.Empty;

    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;

    [JsonProperty("name")] public string Name { get; set; } = string.Empty;

    [JsonProperty("key")] public string Key { get; set; } = string.Empty;

    [JsonProperty("site")] public string Site { get; set; } = string.Empty;

    [JsonProperty("size")] public int Size { get; set; }
    
    [JsonProperty("official")] public bool Official { get; set; }

    [JsonProperty("published_at")] public DateTime? PublishedAt { get; set; }
}