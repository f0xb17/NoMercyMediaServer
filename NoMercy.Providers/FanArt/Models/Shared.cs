using Newtonsoft.Json;

namespace NoMercy.Providers.FanArt.Models;

public class Image
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("url")] public Uri Url { get; set; }
    [JsonProperty("likes")] public int Likes { get; set; }
}

public class CdArt: Image
{
    [JsonProperty("disc")] public int Disc { get; set; }
    [JsonProperty("size")] public int Size { get; set; }
}

public class VideoImage: Image
{
    [JsonProperty("lang")] public string Language { get; set; }
}