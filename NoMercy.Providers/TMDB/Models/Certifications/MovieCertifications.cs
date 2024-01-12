using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;

public class MovieCertifications
{
    [JsonProperty("certifications")]
    public Dictionary<string, List<CertificationMovie>> Certifications { get; set; }
}

public class CertificationMovie
{
    [JsonProperty("certification")]
    public string CertificationCertification { get; set; }

    [JsonProperty("meaning")]
    public string Meaning { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }
}
