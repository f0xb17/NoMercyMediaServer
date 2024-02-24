using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Combined;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class EpisodeAppends : EpisodeDetails
{
    [JsonProperty("credits")] public Credits Credits { get; set; } = new(); 
    [JsonProperty("changes")] public EpisodeChanges Changes { get; set; } = new(); 
    [JsonProperty("external_ids")] public ExternalIds ExternalIds { get; set; } = new(); 
    [JsonProperty("images")] public Images Images { get; set; } = new(); 
    [JsonProperty("translations")] public CombinedTranslations Translations { get; set; } = new(); 
    [JsonProperty("videos")] public Videos Videos { get; set; } = new(); 
}