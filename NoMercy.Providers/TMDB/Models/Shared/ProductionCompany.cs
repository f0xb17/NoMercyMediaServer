using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class ProductionCompany
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("logo_path")] public string LogoPath { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("origin_country")] public string OriginCountry { get; set; }
}