// ReSharper disable All

using NoMercy.Providers.CoverArt.Models;
using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class ReleaseGroupClient : BaseClient
{
    public ReleaseGroupClient(Guid? id, string[]? appendices = null) : base((Guid)id!)
    {
    }
    
    public Task<ReleaseAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["inc"] = string.Join("+", appendices),
            ["fmt"] = "json"
        };
        
        return Get<ReleaseAppends>("release-group/" + Id, queryParams, priority);
    }

    public Task<ReleaseAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "artists",
            "releases",
        ], priority);
    }
    
}

