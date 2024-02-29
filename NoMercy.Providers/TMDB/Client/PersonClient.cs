using NoMercy.Providers.TMDB.Models.People;
// ReSharper disable All

namespace NoMercy.Providers.TMDB.Client;

public class PersonClient : BaseClient
{
    public PersonClient(int? id = 0, string[]? appendices = null) : base((int)id!)
    {}

    public Task<PersonDetails> Details()
    {
        return Get<PersonDetails>("person/" + Id);
    }

    private Task<PersonAppends> WithAppends(string[] appendices)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["append_to_response"] = string.Join(",", appendices)
        };
        
        return Get<PersonAppends>("person/" + Id, queryParams);
    }

    public Task<PersonAppends> WithAllAppends()
    {
        return WithAppends([
            "changes",
            "movie_credits",
            "tv_credits",
            "external_ids",
            "images",
            "translations"
        ]);
    }

    public Task<PersonChanges> Changes(string startDate, string endDate)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["start_date"] = startDate,
            ["end_date"] = endDate
        };
        
        return Get<PersonChanges>("person/" + Id + "/changes", queryParams);
    }

    public Task<PersonMovieCredits> MovieCredits()
    {
        return Get<PersonMovieCredits>("person/" + Id + "/movie_credits");
    }

    public Task<PersonTvCredits> TvCredits()
    {
        return Get<PersonTvCredits>("person/" + Id + "/tv_credits");
    }

    public Task<PersonExternalIds> ExternalIds()
    {
        return Get<PersonExternalIds>("person/" + Id + "/external_ids");
    }

    public Task<PersonImages> Images()
    {
        return Get<PersonImages>("person/" + Id + "/images");
    }

    public Task<PersonTranslations> Translations()
    {
        return Get<PersonTranslations>("person/" + Id + "/translations");
    }
    
    public Task<List<Person>> Popular(int limit = 10)
    {
        return Paginated<Person>("person/popular", limit);
    }

}