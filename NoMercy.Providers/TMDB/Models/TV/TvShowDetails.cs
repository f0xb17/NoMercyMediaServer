using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Networks;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.TV;

public class TvShowDetails : TvShow
{
    [JsonProperty("adult")] public bool Adult { get; set; }

    [JsonProperty("created_by")] public List<CreatedBy> CreatedBy { get; set; } = new();

    [JsonProperty("episode_run_time")] public int[] EpisodeRunTime { get; set; } = [];

    [JsonProperty("genres")] public List<Genre> Genres { get; set; } = new();

    [JsonProperty("homepage")] public Uri? Homepage { get; set; }

    [JsonProperty("in_production")] public bool InProduction { get; set; }

    [JsonProperty("languages")] public string[] Languages { get; set; } = [];

    [JsonProperty("last_episode_to_air")] public Episode.Episode? LastEpisodeToAir { get; set; }

    [JsonProperty("next_episode_to_air")] public Episode.Episode? NextEpisodeToAir { get; set; }

    [JsonProperty("networks")] public List<Network> Networks { get; set; } = new();

    [JsonProperty("number_of_episodes")] public int NumberOfEpisodes { get; set; }

    [JsonProperty("number_of_seasons")] public int NumberOfSeasons { get; set; }

    [JsonProperty("production_companies")] public List<Network> ProductionCompanies { get; set; } = new();

    [JsonProperty("production_countries")] public List<ProductionCountry> ProductionCountries { get; set; } = new();

    [JsonProperty("seasons")] public List<Season.Season> Seasons { get; set; } = new();

    [JsonProperty("spoken_languages")] public List<SpokenLanguage> SpokenLanguages { get; set; } = new();

    [JsonProperty("status")] public string? Status { get; set; }

    [JsonProperty("tagline")] public string? Tagline { get; set; }

    [JsonProperty("type")] public string? Type { get; set; }
}