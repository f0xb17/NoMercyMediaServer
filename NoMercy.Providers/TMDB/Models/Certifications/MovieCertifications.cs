using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;

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
