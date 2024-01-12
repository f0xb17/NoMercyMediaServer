using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Client;

public class EpisodeClient : BaseClient
{
    private readonly int _episodeNumber;
    private readonly int _seasonNumber;

    public EpisodeClient(int id, int seasonNumber, int episodeNumber, string[]? appendices = null) : base(id)
    {
        _seasonNumber = seasonNumber;
        _episodeNumber = episodeNumber;
    }

    public Task<EpisodeDetails> Details()
    {
        return Get<EpisodeDetails>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber);
    }

    public Task<EpisodeAppends> WithAppends(string[] appendices)
    {
        var @params = new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        };

        return Get<EpisodeAppends>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber);
    }

    public Task<EpisodeAppends> WithAllAppends()
    {
        return WithAppends([
            "changes",
            "credits",
            "external_ids",
            "images",
            "translations",
            "videos"
        ]);
    }

    public Task<EpisodeChanges> Changes(string startDate, string endDate)
    {
        return Get<EpisodeChanges>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/changes", new Dictionary<string, string>
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate
        });
    }

    public Task<Credits> Credits()
    {
        return Get<Credits>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/credits");
    }

    public Task<ExternalIds> ExternalIds()
    {
        return Get<ExternalIds>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/external_ids");
    }

    public Task<Images> Images()
    {
        return Get<Images>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/images");
    }

    public Task<SharedTranslations> Translations()
    {
        return Get<SharedTranslations>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/translations");
    }

    public Task<Videos> Videos()
    {
        return Get<Videos>("tv/" + Id + "/season/" + _seasonNumber + "/episode/" + _episodeNumber + "/videos");
    }
}