using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackGet
{
    [JsonProperty("track")] public Track Track;
}