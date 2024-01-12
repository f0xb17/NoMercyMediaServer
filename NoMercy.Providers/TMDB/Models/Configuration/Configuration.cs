using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Configuration;

public class Configuration
{
    [JsonProperty("images")] public Image Images { get; set; } = new();
    [JsonProperty("change_keys")] public List<string> ChangeKeys { get; set; } = new();
    
    public class Image
    {
        [JsonProperty("base_url")] public string BaseUrl { get; set; } = string.Empty;
        [JsonProperty("secure_base_url")] public string SecureBaseUrl { get; set; } = string.Empty;
        [JsonProperty("backdrop_sizes")] public List<string> BackdropSizes { get; set; } = new();
        [JsonProperty("logo_sizes")] public string[] LogoSizes { get; set; } = [];
        [JsonProperty("poster_sizes")] public string[] PosterSizes { get; set; } = [];
        [JsonProperty("profile_sizes")] public string[] ProfileSizes { get; set; } = [];
        [JsonProperty("still_sizes")] public string[] StillSizes { get; set; } = [];
    }

}