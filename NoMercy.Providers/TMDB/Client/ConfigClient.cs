using NoMercy.Providers.TMDB.Models.Configuration;
// ReSharper disable All

namespace NoMercy.Providers.TMDB.Client;

public class ConfigClient : BaseClient
{

    public Task<Configuration?> Configuration()
    {
        return Get<Configuration>("configuration");
    }
    
    public Task<List<Language>?> Languages()
    {
        return Get<List<Language>>("configuration/languages");
    }

    public Task<List<Country>?> Countries()
    {
        return Get<List<Country>>("configuration/countries");
    }
    
    public Task<List<Job>?> Jobs()
    {
        return Get<List<Job>>("configuration/jobs");
    }
    
    public Task<List<string>?> PrimaryTranslations()
    {
        return Get<List<string>>("configuration/primary_translations");
    }
    
    public Task<List<Timezone>?> Timezones()
    {
        return Get<List<Timezone>>("configuration/timezones");
    }

}