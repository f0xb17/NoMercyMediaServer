using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class MovieCertifications
{
    [JsonProperty("certifications")]
    public Dictionary<string, MovieCertification[]> Certifications { get; set; } = new();
}

public class MovieCertification
{
    [JsonProperty("certification")]
    public string Rating { get; set; }

    [JsonProperty("meaning")]
    public string Meaning { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }
}
