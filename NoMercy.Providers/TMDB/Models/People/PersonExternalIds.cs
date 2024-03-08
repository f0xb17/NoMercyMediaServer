using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonExternalIds
{
    [JsonProperty("imdb_id", NullValueHandling = NullValueHandling.Ignore)] public string? ImdbId { get; set; }
    [JsonProperty("facebook_id", NullValueHandling = NullValueHandling.Ignore)] public string? FacebookId { get; set; }
    [JsonProperty("freebase_mid", NullValueHandling = NullValueHandling.Ignore)] public string? FreebaseMid { get; set; }
    [JsonProperty("freebase_id", NullValueHandling = NullValueHandling.Ignore)] public string? FreebaseId { get; set; }
    [JsonProperty("twitter_id", NullValueHandling = NullValueHandling.Ignore)] public string? TwitterId { get; set; }
    [JsonProperty("tvrage_id", NullValueHandling = NullValueHandling.Ignore)] public string? TvRageId { get; set; }
    [JsonProperty("wikidata_id", NullValueHandling = NullValueHandling.Ignore)] public string? WikipediaId { get; set; }
    [JsonProperty("instagram_id", NullValueHandling = NullValueHandling.Ignore)] public string? InstagramId { get; set; }
    [JsonProperty("tiktok_id", NullValueHandling = NullValueHandling.Ignore)] public string? TikTokId { get; set; }
    [JsonProperty("youtube_id", NullValueHandling = NullValueHandling.Ignore)] public string? YoutubeId { get; set; }
}