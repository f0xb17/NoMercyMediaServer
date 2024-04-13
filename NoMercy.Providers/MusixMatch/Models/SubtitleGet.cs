using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class SubtitleGet
{
    [JsonProperty("message")] public SubtitleGetMessage Message { get; set; }
}

public class SubtitleGetMessage
{
    [JsonProperty("header")] public SubtitleGetMessageHeader Header { get; set; }
    [JsonProperty("body")] public SubtitleGetMessageBody Body { get; set; }
}

public class SubtitleGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("execute_time")] public double ExecuteTime { get; set; }
    [JsonProperty("pid")] public long Pid { get; set; }
    [JsonProperty("surrogate_key_list")] public object[] SurrogateKeyList { get; set; }
}

public class SubtitleGetMessageBody
{
    [JsonProperty("macro_calls")] public MacroCalls MacroCalls { get; set; }
}