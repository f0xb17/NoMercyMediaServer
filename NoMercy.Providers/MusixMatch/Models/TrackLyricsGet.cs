using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackLyricsGet
{
    [JsonProperty("message")] public TrackLyricsGetMessage Message { get; set; }
}

public class TrackLyricsGetMessage
{
    [JsonProperty("header")] public TrackLyricsGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public TrackLyricsGetMessagedBody Body { get; set; }
}

public class TrackLyricsGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
}

public class TrackLyricsGetMessagedBody
{
    [JsonProperty("lyrics")] public Lyrics Lyrics { get; set; }
}
