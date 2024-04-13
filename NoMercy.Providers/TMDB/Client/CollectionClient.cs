using NoMercy.Providers.TMDB.Models.Collections;
// ReSharper disable All

namespace NoMercy.Providers.TMDB.Client;

public class CollectionClient : BaseClient
{
    public CollectionClient(int id, string[]? appendices = null) : base(id)
    {
    }

    public Task<CollectionDetails?> Details()
    {
        return Get<CollectionDetails>("collection/" + Id);
    }

    private Task<CollectionAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        };
        
        return Get<CollectionAppends>("collection/" + Id, queryParams, priority);
    }

    public Task<CollectionAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "images",
            "translations"
        ], priority);
    }

    public Task<CollectionImages?> Images()
    {
        return Get<CollectionImages>("collection/" + Id + "/images");
    }

    public Task<CollectionsTranslations?> Translations()
    {
        return Get<CollectionsTranslations>("collection/" + Id + "/translations");
    }
}