using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class TrackSnippetGet
{
    [JsonProperty("message")] public TrackSnippetGetMessage Message { get; set; }
}

public class TrackSnippetGetMessage
{
    [JsonProperty("header")] public TrackSnippetGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public TrackSnippetGetMessageBody Body { get; set; }
}

public class TrackSnippetGetMessageBody
{
    [JsonProperty("snippet")] public Snippet Snippet { get; set; }
}

public class TrackSnippetGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
}