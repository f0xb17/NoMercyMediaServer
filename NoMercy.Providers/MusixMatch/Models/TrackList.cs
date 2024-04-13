using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackList
{
    [JsonProperty("track")] public Track Track;
}
