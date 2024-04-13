// ReSharper disable All

using NoMercy.Providers.CoverArt.Models;
using NoMercy.Providers.MusicBrainz.Models;

namespace NoMercy.Providers.MusicBrainz.Client;

public class ReleaseClient : BaseClient
{
    public ReleaseClient(Guid? id, string[]? appendices = null) : base((Guid)id!)
    {
    }
    
    public Task<ReleaseAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["inc"] = string.Join("+", appendices),
            ["fmt"] = "json"
        };
        
        return Get<ReleaseAppends>("release/" + Id, queryParams, priority);
    }

    public Task<ReleaseAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "artists",
            "labels",
            "recordings",
            "release-groups",
            "media",
            "artist-credits",
            "discids",
            "puids",
            "isrcs",
            "artist-rels",
            "label-rels",
            "recording-rels",
            "release-rels",
            "release-group-rels",
            "url-rels",
            "work-rels",
            "recording-level-rels",
            "work-level-rels",
            "annotation",
            "aliases",
            "artist-credits",
            "collections",
            "genres",
            "tags",
        ], priority);
    }
    
}

