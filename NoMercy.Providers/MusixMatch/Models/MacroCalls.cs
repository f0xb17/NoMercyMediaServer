using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MacroCalls
{
    [JsonProperty("track.lyrics.get")] public TrackLyricsGet TrackLyricsGet { get; set; }
    [JsonProperty("track.snippet.get")] public TrackSnippetGet TrackSnippetGet { get; set; }
    [JsonProperty("track.subtitles.get")] public TrackSubtitlesGet TrackSubtitlesGet { get; set; }
    [JsonProperty("userblob.get")] public UserblobGet UserblobGet { get; set; }
    [JsonProperty("matcher.track.get")] public MatcherTrackGet MatcherTrackGet { get; set; }
}