using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackSearch
{
    [JsonProperty("track_list")] public List<TrackList> Results;
}
