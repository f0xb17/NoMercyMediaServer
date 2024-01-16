using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvContentRatings
{
    [JsonProperty("certifications")]
    public Dictionary<string, TvContentRating[]> ContentRating { get; set; } = new();
}

public class TvContentRating
{
    [JsonProperty("certification")]
    public string CertificationCertification { get; set; }

    [JsonProperty("meaning")]
    public string Meaning { get; set; }

    [JsonProperty("order")]
    public long Order { get; set; }
}