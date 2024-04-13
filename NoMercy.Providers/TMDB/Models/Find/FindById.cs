using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Providers.TMDB.Models.Find;

public class FindById
{
    [JsonProperty("movie_results")] public Movie[] MovieResults { get; set; } = [];
    [JsonProperty("person_results")] public Person[] PersonResults { get; set; } = [];
    [JsonProperty("tv_results")] public TvShow[] TvResults { get; set; } = [];
    [JsonProperty("tv_episode_results")] public Episode.Episode[] TvEpisodeResults { get; set; } = [];
    [JsonProperty("tv_season_results")] public Season.Season[] TvSeasonResults { get; set; } = [];
}