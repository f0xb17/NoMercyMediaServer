using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Networks;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.Search;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Providers.TMDB.Client;

public class SearchClient : BaseClient
{
    public Task<PaginatedResponse<Movie>?> Movie(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Movie>>("search/movie", queryParams);
    }
    
    public Task<PaginatedResponse<TvShow>?> TvShow(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["first_air_date_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<TvShow>>("search/tv", queryParams);
    }

    public Task<PaginatedResponse<Person>?> Person(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Person>>("search/person", queryParams);
    }

    public Task<PaginatedResponse<Multi>?> Multi(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Multi>>("search/multi", queryParams);
    }
    
    public Task<PaginatedResponse<Collection>?> Collection(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Collection>>("search/collection", queryParams);
    }

    public Task<PaginatedResponse<Network>?> Network(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Network>>("search/network", queryParams);
    }
    
    public Task<PaginatedResponse<Keyword>?> Keyword(string query, string year)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["query"] = query,
            ["primary_release_year"] = year,
            ["include_adult"] = "false"
        };
        
        return Get<PaginatedResponse<Keyword>>("search/keyword", queryParams);
    }

}