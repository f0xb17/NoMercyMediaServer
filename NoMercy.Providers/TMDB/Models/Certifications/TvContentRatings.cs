using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Certifications;

public class TvShowCertifications
{
    [JsonProperty("certifications")]
    public Dictionary<string, List<TvShowCertification>> Certifications { get; set; }
}

public class TvShowCertification
{
    [JsonProperty("certification")]
    public string CertificationCertification { get; set; }

    [JsonProperty("meaning")]
    public string Meaning { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }
}