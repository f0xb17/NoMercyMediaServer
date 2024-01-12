using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Companies;

public class CompanyImages
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("logos")] public List<Logo> Logos { get; set; } = new();
}