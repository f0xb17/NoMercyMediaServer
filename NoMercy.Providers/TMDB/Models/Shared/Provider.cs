using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Shared;

public class Provider
{
    [JsonProperty("display_priority")] public int DisplayPriority { get; set; }

    [JsonProperty("logo_path")] public string LogoPath { get; set; }

    [JsonProperty("provider_name")] public string ProviderName { get; set; }

    [JsonProperty("provider_id")] public int ProviderId { get; set; }
}