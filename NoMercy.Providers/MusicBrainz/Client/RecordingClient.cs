// ReSharper disable All

using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class RecordingClient : BaseClient
{
    public RecordingClient(Guid? id, string[]? appendices = null) : base((Guid)id!)
    {
    }
    
    public Task<RecordingAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["inc"] = string.Join("+", appendices),
            ["fmt"] = "json"
        };
        
        return Get<RecordingAppends>("recording/" + Id, queryParams, priority);
    }

    public Task<RecordingAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "artist-credits",
            "artists",
            "releases",
            "tags",
            "genres",
        ], priority);
    }
    
}

