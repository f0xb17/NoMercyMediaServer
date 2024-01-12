using NoMercy.Providers.Helpers;
using NoMercy.Providers.TMDB.Models.Certifications;
using NoMercy.Providers.TMDB.Models.Genres;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Providers.TMDB.Client;

public class TvClient : BaseClient
{
    public TvClient(int? id = 0, string[]? appendices = null) : base((int)id!)
    {
    }

    public SeasonClient Season(int seasonNumber, string[]? items = null)
    {
        return new SeasonClient(Id, seasonNumber, items);
    }

    //public Task<Models.Season.SeasonAppends> SeasonWithAppends(int SeasonNumber, string[] Appendices)
    //{
    //	return (new SeasonClient(Id, SeasonNumber)).WithAppends(Appendices);
    //}

    public Task<TvShowDetails> Details()
    {
        return Get<TvShowDetails>("tv/" + Id);
    }

    public Task<TvShowAppends> WithAppends(string[] appendices)
    {
        return Get<TvShowAppends>("tv/" + Id, new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        });
    }

    public Task<TvShowAppends> WithAllAppends()
    {
        return WithAppends([
            "aggregate_credits",
            "alternative_titles",
            "changes",
            "content_ratings",
            "credits",
            "external_ids",
            "images",
            "keywords",
            "recommendations",
            "similar",
            "translations",
            "videos",
            "watch/providers"
        ]);
    }

    public Task<TvAggregatedCredits> AggregatedCredits()
    {
        return Get<TvAggregatedCredits>("tv/" + Id + "/aggregate_credits");
    }

    public Task<TvAlternativeTitles> AlternativeTitles()
    {
        return Get<TvAlternativeTitles>("tv/" + Id + "/alternative_titles");
    }

    public Task<TvChanges> Changes(string startDate, string endDate)
    {
        return Get<TvChanges>("tv/" + Id + "/changes", new Dictionary<string, string>
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate
        });
    }

    public Task<TvContentRatings> ContentRatings()
    {
        return Get<TvContentRatings>("tv/" + Id + "/content_ratings");
    }

    public Task<TvCredits> Credits()
    {
        return Get<TvCredits>("tv/" + Id + "/credits");
    }

    public Task<TvEpisodeGroups> EpisodeGroups()
    {
        return Get<TvEpisodeGroups>("tv/" + Id + "/episode_groups");
    }

    public Task<TvExternalIds> ExternalIds()
    {
        return Get<TvExternalIds>("tv/" + Id + "/external_ids");
    }

    public Task<TvImages> Images()
    {
        return Get<TvImages>("tv/" + Id + "/images");
    }

    public Task<TvKeywords> Keywords()
    {
        return Get<TvKeywords>("tv/" + Id + "/keywords");
    }

    public Task<TvRecommendations> Recommendations()
    {
        return Get<TvRecommendations>("tv/" + Id + "/recommendations");
    }

    public Task<TvReviews> Reviews()
    {
        return Get<TvReviews>("tv/" + Id + "/reviews");
    }

    public Task<TvScreenedTheatrically> ScreenedTheatrically()
    {
        return Get<TvScreenedTheatrically>("tv/" + Id + "/screened_theatrically");
    }

    public Task<TvSimilar> Similar()
    {
        return Get<TvSimilar>("tv/" + Id + "/similar");
    }

    public Task<SharedTranslations> Translations()
    {
        return Get<SharedTranslations>("tv/" + Id + "/translations");
    }

    public Task<TvVideos> Videos()
    {
        return Get<TvVideos>("tv/" + Id + "/videos");
    }

    public Task<WatchProviders> WatchProviders()
    {
        return Get<WatchProviders>("tv/" + Id + "/watch/providers");
    }

    public Task<TvShowLatest> Latest()
    {
        return Get<TvShowLatest>("tv/latest");
    }

    public Task<TvAiringToday> AiringToday()
    {
        return Get<TvAiringToday>("tv/" + Id + "/airing_today");
    }

    public Task<TvOnTheAir> OnTheAir()
    {
        return Get<TvOnTheAir>("tv/on_the_air");
    }

    public Task<List<TvShow>> Popular(int limit = 10)
    {
        return Paginated<TvShow>("tv/popular", limit);
    }

    public Task<TvTopRated> TopRated()
    {
        return Get<TvTopRated>("tv/top_rated");
    }

    public Task<TvShowCertifications> Certifications()
    {
        return Get<TvShowCertifications>("certification/tv/list");
    }
    
    public Task<GenreTv> Genres()
    {
        return Get<GenreTv>("genre/tv/list");
    }
}