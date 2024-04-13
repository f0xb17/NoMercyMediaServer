using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackRichsyncGet
{
    [JsonProperty("richsync")] public Richsync Richsync;
}