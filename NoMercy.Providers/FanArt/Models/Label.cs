using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;

public class Label {
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("musiclabel")] public MusicLabel[] Labels { get; set; }
}

public class MusicLabel {
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("url")] public Uri Url { get; set; }
    [JsonProperty("colour")] public string Color { get; set; }
    [JsonProperty("likes")] public string Likes { get; set; }
}