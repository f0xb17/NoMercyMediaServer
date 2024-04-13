// ReSharper disable All

using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class ArtistClient : BaseClient
{
    public ArtistClient(Guid? id, string[]? appendices = null) : base((Guid)id!)
    {
    }
    
    public Task<ArtistAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["inc"] = string.Join("+", appendices),
            ["fmt"] = "json"
        };
        
        return Get<ArtistAppends>("artist/" + Id, queryParams, priority);
    }

    public Task<ArtistAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "genres",
            "recordings",
            "releases",
            "release-groups",
            "works",
        ], priority);
    }
    
}

