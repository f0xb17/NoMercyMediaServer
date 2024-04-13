using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MatcherTrackGet
{
    [JsonProperty("message")] public MatcherTrackGetMessage Message { get; set; }
}

public class MatcherTrackGetMessage
{
    [JsonProperty("header")] public  MatcherTrackGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public  MatcherTrackGetMessageBody Body { get; set; }
}

public class  MatcherTrackGetMessageBody
{
    [JsonProperty("track")] public Track Track { get; set; }
}

public class  MatcherTrackGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
    [JsonProperty("confidence")] public long Confidence { get; set; }
    [JsonProperty("mode")] public string Mode { get; set; }
    [JsonProperty("cached")] public long Cached { get; set; }
}

