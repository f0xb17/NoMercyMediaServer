using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.MusixMatch.Models;
using static System.String;

namespace NoMercy.Providers.MusixMatch.Client;

public class MusixmatchClient : BaseClient
{
    public MusixmatchClient(Guid id) : base(id)
    {
    }

    public Task<SubtitleGet?> SongSearch(TrackSearchParameters trackParameters, bool priority = false)
    {
        var additionalArguments = new Dictionary<string, string?>
        {
            ["q_artist"] = trackParameters.Artist,
            ["q_artists"] = Join(",", trackParameters.Artists ?? []),
            ["q_track"] = trackParameters.Title,
            // ["q_album"] = trackParameters.Album,
            ["q_duration"] = trackParameters.Duration,
        };
        
        return Get<SubtitleGet>("macro.subtitles.get", additionalArguments, priority);
    }
}