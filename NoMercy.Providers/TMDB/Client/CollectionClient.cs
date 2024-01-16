using NoMercy.Providers.TMDB.Models.Collections;

namespace NoMercy.Providers.TMDB.Client;

public class CollectionClient : BaseClient
{
    public CollectionClient(int id, string[]? appendices = null) : base(id)
    {
    }

    public Task<CollectionDetails> Details()
    {
        return Get<CollectionDetails>("collection/" + Id);
    }

    public Task<CollectionAppends> WithAppends(string[] appendices)
    {
        var @prarams = new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        };
        
        return Get<CollectionAppends>("collection/" + Id, @prarams);
    }

    public Task<CollectionAppends> WithAllAppends()
    {
        return WithAppends([
            "images",
            "translations"
        ]);
    }

    public Task<CollectionImages> Images()
    {
        return Get<CollectionImages>("collection/" + Id + "/images");
    }

    public Task<CollectionsTranslations> Translations()
    {
        return Get<CollectionsTranslations>("collection/" + Id + "/translations");
    }
}