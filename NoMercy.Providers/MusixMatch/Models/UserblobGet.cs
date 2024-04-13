using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class UserblobGet
{
    [JsonProperty("message")] public UserblobGetMessage Message { get; set; }
    [JsonProperty("meta")] public UserblobGetMeta Meta { get; set; }
}

public class UserblobGetMessage
{
    [JsonProperty("header")] public UserblobGetMessageHeader Header { get; set; }
}

public class UserblobGetMessageHeader
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
}

public class UserblobGetMeta
{
    [JsonProperty("status_code")] public long StatusCode { get; set; }
    [JsonProperty("last_updated")] public DateTimeOffset LastUpdated { get; set; }
}
