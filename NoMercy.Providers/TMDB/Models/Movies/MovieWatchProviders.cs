using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieWatchProviders
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public Countries<PaymentType> Results { get; set; } = new();
}

public class PaymentType
{
    [JsonProperty("link")] public Uri Link { get; set; } = null!;

    [JsonProperty("flatrate")] public List<PaymentDetails> FlatRate { get; set; } = new();

    [JsonProperty("rent")] public List<PaymentDetails> Rent { get; set; } = new();

    [JsonProperty("buy")] public List<PaymentDetails> Buy { get; set; } = new();
}

public class PaymentDetails
{
    [JsonProperty("display_priority")] public int DisplayPriority { get; set; }

    [JsonProperty("logo_path")] public string LogoPath { get; set; } = string.Empty;

    [JsonProperty("provider_id")] public int ProviderId { get; set; }

    [JsonProperty("provider_name")] public string ProviderName { get; set; } = string.Empty;
}