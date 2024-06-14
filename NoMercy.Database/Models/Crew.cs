#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(CreditId), nameof(MovieId), nameof(JobId), IsUnique = true)]
[Index(nameof(CreditId), nameof(TvId), nameof(JobId), IsUnique = true)]
[Index(nameof(CreditId), nameof(SeasonId), nameof(JobId), IsUnique = true)]
[Index(nameof(CreditId), nameof(EpisodeId), nameof(JobId), IsUnique = true)]
public class Crew
{
    private readonly TmdbMovie _tmdbMovie;
    private readonly TmdbTvShowAppends _tmdbTv;
    private readonly TmdbSeason _tmdbSeason;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("credit_id")] public string? CreditId { get; set; }

    [JsonProperty("movie_id")] public int? MovieId { get; set; }
    public Movie? Movie { get; set; }

    [JsonProperty("tv_id")] public int? TvId { get; set; }
    public Tv? Tv { get; set; }

    [JsonProperty("season_id")] public int? SeasonId { get; set; }
    public Season Season { get; set; }

    [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
    public Episode? Episode { get; set; }

    [JsonProperty("person_id")] public int? PersonId { get; set; }
    public Person Person { get; set; }

    [JsonProperty("job_id")] public int? JobId { get; set; }
    public Job Job { get; set; }

    public Crew()
    {
    }

    public Crew(TmdbCrew tmdbCrew, TmdbMovie tmdbMovieAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Job> jobs)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCrew.CreditId;
        PersonId = tmdbCrew.Id;
        MovieId = tmdbMovieAppends.Id;
        JobId = jobs.FirstOrDefault(r => r.CreditId == tmdbCrew.CreditId)?.Id;
    }

    public Crew(TmdbCrew tmdbCrew, TmdbTvShowAppends tmdbTvAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Job> jobs)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCrew.CreditId;
        PersonId = tmdbCrew.Id;
        TvId = tmdbTvAppends.Id;
        JobId = jobs.FirstOrDefault(r => r.CreditId == tmdbCrew.CreditId)?.Id;
    }

    public Crew(TmdbCrew tmdbCrew, TmdbSeason tmdbSeasonAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Job> jobs)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCrew.CreditId;
        PersonId = tmdbCrew.Id;
        SeasonId = tmdbSeasonAppends.Id;
        JobId = jobs.FirstOrDefault(r => r.CreditId == tmdbCrew.CreditId)?.Id;
    }

    public Crew(TmdbCrew tmdbCrew, TmdbEpisode tmdbEpisodeAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Job> jobs)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCrew.CreditId;
        PersonId = tmdbCrew.Id;
        EpisodeId = tmdbEpisodeAppends.Id;
        JobId = jobs.FirstOrDefault(r => r.CreditId == tmdbCrew.CreditId)?.Id;
    }
}