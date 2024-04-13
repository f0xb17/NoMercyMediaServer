using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackSubtitlesGet
{
    [JsonProperty("message")] public TrackSubtitlesGetMessage Message { get; set; }
}

public class TrackSubtitlesGetMessage
{
    [JsonProperty("header")] public TrackSubtitlesGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public TrackSubtitlesGetMessageBody Body { get; set; }
}

public class TrackSubtitlesGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("available")] public long Available { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
    [JsonProperty("instrumental")] public long Instrumental { get; set; }
}

public class TrackSubtitlesGetMessageBody
{
    [JsonProperty("subtitle_list")] public SubtitleList[] SubtitleList { get; set; }
}

public class SubtitleList
{
    [JsonProperty("subtitle")] public Subtitle Subtitle { get; set; }
}

