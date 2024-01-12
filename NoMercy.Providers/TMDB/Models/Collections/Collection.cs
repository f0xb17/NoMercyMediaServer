using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Collections;

public class Collection(int id, string name, string posterPath, string backdropPath)
{
    [JsonProperty("id")] public int Id { get; set; } = id;

    [JsonProperty("name")] public string Name { get; set; } = name;

    [JsonProperty("poster_path")] public string PosterPath { get; set; } = posterPath;

    [JsonProperty("backdrop_path")] public string BackdropPath { get; set; } = backdropPath;
}