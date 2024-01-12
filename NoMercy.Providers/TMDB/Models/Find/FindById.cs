using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Providers.TMDB.Models.Find;

public class FindById
{
    [JsonProperty("movie_results")] public List<Movie> MovieResults { get; set; } = new();

    [JsonProperty("person_results")] public List<Person> PersonResults { get; set; } = new();

    [JsonProperty("tv_results")] public List<TvShow> TvResults { get; set; } = new();

    [JsonProperty("tv_episode_results")] public List<Episode.Episode> TvEpisodeResults { get; set; } = new();

    [JsonProperty("tv_season_results")] public List<Season.Season> TvSeasonResults { get; set; } = new();
}