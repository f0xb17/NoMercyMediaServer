using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchTrackGet
{
    [JsonProperty("track")] public MusixMatchMusixMatchTrack MusixMatchMusixMatchTrack { get; set; } = new();
}
