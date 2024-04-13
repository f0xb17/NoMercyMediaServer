#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;

public class ArtistDetails: Artist {
    [JsonProperty("artistbackground")] public Image[] Backgrounds { get; set; }
    [JsonProperty("hdmusiclogo")] public Image[] HdLogos { get; set; }
    [JsonProperty("artistthumb")] public Image[] Thumbs { get; set; }
    [JsonProperty("musiclogo")] public Image[] Logos { get; set; }
    [JsonProperty("musicbanner")] public Image[] Banners { get; set; }
}

public class Artist {
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("mbid_id")] public string MbId { get; set; }
    [JsonProperty("albums")] public Dictionary<string, Artists> Artists { get; set; }
}

public class Artists {
    [JsonProperty("cdart")] public Image[] CdArt { get; set; }
    [JsonProperty("albumcover")] public Image[] AlbumCover { get; set; }
}
