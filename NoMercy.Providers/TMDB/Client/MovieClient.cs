using NoMercy.Providers.TMDB.Models.Genres;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Shared;
using MovieCertifications = NoMercy.Providers.TMDB.Models.Certifications.MovieCertifications;
// ReSharper disable All

namespace NoMercy.Providers.TMDB.Client;

public class MovieClient : BaseClient
{
    public MovieClient(int? id = 0, string[]? appendices = null) : base((int)id!)
    {
    }

    public Task<MovieDetails?> Details()
    {
        return Get<MovieDetails>("movie/" + Id);
    }

    private Task<MovieAppends?> WithAppends(string[] appendices, bool? priority = false)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        };
        
        return Get<MovieAppends>("movie/" + Id, queryParams, priority);
    }

    public Task<MovieAppends?> WithAllAppends(bool? priority = false)
    {
        return WithAppends([
            "alternative_titles",
            "release_dates",
            "changes",
            "credits",
            "keywords",
            "recommendations",
            "similar",
            "translations",
            "external_ids",
            "videos",
            "images",
            "watch/providers"
        ], priority);
    }

    public Task<MovieAggregatedCredits?> AggregatedCredits()
    {
        return Get<MovieAggregatedCredits>("movie/" + Id + "/aggregate_credits");
    }

    public Task<MovieAlternativeTitles?> AlternativeTitles()
    {
        return Get<MovieAlternativeTitles>("movie/" + Id + "/alternative_titles");
    }

    public Task<MovieChanges?> Changes(string startDate, string endDate)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate
        };

        return Get<MovieChanges>("movie/" + Id + "/changes", queryParams);
    }

    public Task<MovieCredits?> Credits()
    {
        return Get<MovieCredits>("movie/" + Id + "/credits");
    }

    public Task<MovieExternalIds?> ExternalIds()
    {
        return Get<MovieExternalIds>("movie/" + Id + "/external_ids");
    }

    public Task<MovieImages?> Images()
    {
        return Get<MovieImages>("movie/" + Id + "/images");
    }

    public Task<MovieKeywords?> Keywords()
    {
        return Get<MovieKeywords>("movie/" + Id + "/keywords");
    }

    public Task<MovieLists?> Lists()
    {
        return Get<MovieLists>("movie/" + Id + "/lists");
    }

    public Task<MovieRecommendations?> Recommendations()
    {
        return Get<MovieRecommendations>("movie/" + Id + "/recommendations");
    }

    public Task<MovieReleaseDates?> ReleaseDates()
    {
        return Get<MovieReleaseDates>("movie/" + Id + "/release_dates");
    }

    public Task<MovieReviews?> Reviews()
    {
        return Get<MovieReviews>("movie/" + Id + "/reviews");
    }

    public Task<MovieSimilar?> Similar()
    {
        return Get<MovieSimilar>("movie/" + Id + "/similar");
    }

    public Task<SharedTranslations?> Translations()
    {
        return Get<SharedTranslations>("movie/" + Id + "/translations");
    }

    public Task<MovieVideos?> Videos()
    {
        return Get<MovieVideos>("movie/" + Id + "/videos");
    }

    public Task<MovieWatchProviders?> WatchProviders()
    {
        return Get<MovieWatchProviders>("movie/" + Id + "/watch/providers");
    }

    public Task<MovieLatest?> Latest()
    {
        return Get<MovieLatest>("movie/" + Id + "/latest");
    }

    public Task<MovieNowPlaying?> NowPlaying()
    {
        return Get<MovieNowPlaying>("movie/" + Id + "/now_playing");
    }

    public Task<List<Movie>?> Popular(int limit = 10)
    {
        return Paginated<Movie>("movie/popular", limit);
    }

    public Task<MovieTopRated?> TopRated()
    {
        return Get<MovieTopRated>("movie/" + Id + "/top_rated");
    }

    public Task<MovieUpcoming?> Upcoming()
    {
        return Get<MovieUpcoming>("movie/" + Id + "/upcoming");
    }

    public Task<MovieCertifications?> Certifications()
    {
        return Get<MovieCertifications>("certification/movie/list");
    }
    
    public Task<GenreMovies?> Genres()
    {
        return Get<GenreMovies>("genre/movie/list");
    }
}