#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
[Index(nameof(CreditId), nameof(MovieId), nameof(RoleId), IsUnique = true)]
[Index(nameof(CreditId), nameof(TvId), nameof(RoleId), IsUnique = true)]
[Index(nameof(CreditId), nameof(SeasonId), nameof(RoleId), IsUnique = true)]
[Index(nameof(CreditId), nameof(EpisodeId), nameof(RoleId), IsUnique = true)]
// [Index(nameof(Id),nameof(PersonId), IsUnique = true)]
public class Cast
{
    private readonly TmdbMovie _tmdbMovie;
    private readonly TmdbTvShowAppends _tmdbTv;
    private readonly TmdbSeason _tmdbSeason;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("credit_id")] public string? CreditId { get; set; }

    [JsonProperty("person_id")] public int PersonId { get; set; }
    public Person Person { get; set; }

    [JsonProperty("movie_id")] public int? MovieId { get; set; }
    public Movie? Movie { get; set; }

    [JsonProperty("tv_id")] public int? TvId { get; set; }
    public Tv? Tv { get; set; }

    [JsonProperty("season_id")] public int? SeasonId { get; set; }
    public Season Season { get; set; }

    [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
    public Episode? Episode { get; set; }

    [JsonProperty("role_id")] public int? RoleId { get; set; }
    public Role Role { get; set; }

    public Cast()
    {
    }

    public Cast(TmdbCast tmdbCast, TmdbMovie tmdbMovieAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Role> roles)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCast.CreditId;
        PersonId = tmdbCast.Id;
        MovieId = tmdbMovieAppends.Id;
        RoleId = roles.Where(r => r.CreditId == tmdbCast.CreditId)?.FirstOrDefault()?.Id;
    }

    public Cast(TmdbCast tmdbCast, TmdbTvShowAppends tmdbTvAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Role> roles)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCast.CreditId;
        PersonId = tmdbCast.Id;
        TvId = tmdbTvAppends.Id;
        RoleId = roles.Where(r => r.CreditId == tmdbCast.CreditId)?.FirstOrDefault()?.Id;
    }

    public Cast(TmdbCast tmdbCast, TmdbSeason tmdbSeasonAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Role> roles)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCast.CreditId;
        PersonId = tmdbCast.Id;
        SeasonId = tmdbSeasonAppends.Id;
        RoleId = roles.Where(r => r.CreditId == tmdbCast.CreditId)?.FirstOrDefault()?.Id;
    }

    public Cast(TmdbCast tmdbCast, TmdbEpisode tmdbEpisodeAppends, TmdbMovie tmdbMovie, TmdbTvShowAppends tmdbTv, TmdbSeason tmdbSeason,
        IEnumerable<Role> roles)
    {
        _tmdbMovie = tmdbMovie;
        _tmdbTv = tmdbTv;
        _tmdbSeason = tmdbSeason;
        CreditId = tmdbCast.CreditId;
        PersonId = tmdbCast.Id;
        EpisodeId = tmdbEpisodeAppends.Id;
        RoleId = roles.Where(r => r.CreditId == tmdbCast.CreditId).FirstOrDefault()?.Id;
    }
}