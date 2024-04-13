#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.CoverArt.Models;

public class Covers
{
    [JsonProperty("images")] public CoverImage[] Images { get; set; }
    [JsonProperty("release")] string Release { get; set; }
}

public class CoverImage
{
    [JsonProperty("approved")] public bool Approved { get; set; }
    [JsonProperty("back")] public bool Back { get; set; }
    [JsonProperty("comment")] public string Comment { get; set; }
    [JsonProperty("edit")] public int Edit { get; set; }
    [JsonProperty("front")] public bool Front { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("image")] public Uri Image { get; set; }
    [JsonProperty("thumbnails")] public Thumbnails Thumbnails { get; set; }
    [JsonProperty("types")] public string[] Types { get; set; }
}

public class Thumbnails
{
    [JsonProperty("250")] public string _250 { get; set; }
    [JsonProperty("500")] public string _500 { get; set; }
    [JsonProperty("1200")] public string _1200 { get; set; }
    [JsonProperty("large")] public string Large { get; set; }
    [JsonProperty("small")] public string Small { get; set; }
}

public enum CoverType
{
    Back,
    BackSpine,
    Booklet,
    Bottom,
    Front,
    Liner,
    Medium,
    Obi,
    Other,
    Poster,
    Spine,
    Sticker,
    Top,
    Track,
    Tray,
    Watermark,
}