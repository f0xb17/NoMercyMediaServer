using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Client;

public class SeasonClient : BaseClient
{
    private readonly int _seasonNumber;

    public SeasonClient(int tvId, int seasonNumber, string[]? appendices = null) : base(tvId)
    {
        _seasonNumber = seasonNumber;
    }

    public EpisodeClient Episode(int episodeNumber, string[]? items = null)
    {
        return new EpisodeClient(Id, _seasonNumber, episodeNumber);
    }

    public Task<SeasonDetails> Details()
    {
        return Get<SeasonDetails>("tv/" + Id + "/season/" + _seasonNumber);
    }

    public Task<SeasonAppends> WithAppends(string[] appendices)
    {
        return Get<SeasonAppends>("tv/" + Id + "/season/" + _seasonNumber, new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        });
    }

    public Task<SeasonAppends> WithAllAppends()
    {
        return WithAppends([
            "aggregate_credits",
            "changes",
            "credits",
            "external_ids",
            "images",
            "translations"
        ]);
    }

    //public Task<AccountStates> AccountStates()
    //{
    //    strreturn Get<Details>(("tv/" + Id + "/season/" + SeasonNumber + "/account_states");
    //    
    //}

    public Task<SeasonAggregatedCredits> AggregatedCredits()
    {
        return Get<SeasonAggregatedCredits>("tv/" + Id + "/season/" + _seasonNumber + "/aggregate_credits");
        
    }

    public Task<SeasonChanges> Changes(string startDate, string endDate)
    {
        var @params = new Dictionary<string, string>
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate
        };

        return Get<SeasonChanges>("tv/" + Id + "/season/" + _seasonNumber + "/changes", @params);
    }

    public Task<Credits> Credits()
    {
        return Get<Credits>("tv/" + Id + "/season/" + _seasonNumber + "/credits");
    }

    public Task<ExternalIds> ExternalIds()
    {
        return Get<ExternalIds>("tv/" + Id + "/season/" + _seasonNumber + "/external_ids");
    }

    public Task<Images> Images()
    {
        return Get<Images>("tv/" + Id + "/season/" + _seasonNumber + "/images");
    }

    public Task<SharedTranslations> Translations()
    {
        return Get<SharedTranslations>("tv/" + Id + "/season/" + _seasonNumber + "/translations");
    }

    public Task<Videos> Videos()
    {
        return Get<Videos>("tv/" + Id + "/season/" + _seasonNumber + "/videos");
    }
}