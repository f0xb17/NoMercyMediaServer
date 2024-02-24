using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TMDBCast = NoMercy.Providers.TMDB.Models.Shared.Cast;
using TMDBMovie = NoMercy.Providers.TMDB.Models.Movies.Movie;
using TMDBTv = NoMercy.Providers.TMDB.Models.TV.TvShowAppends;
using TMDBSeason = NoMercy.Providers.TMDB.Models.Season.Season;
using TMDBEpisode = NoMercy.Providers.TMDB.Models.Episode.Episode;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(CreditId), nameof(MovieId), nameof(RoleId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(TvId), nameof(RoleId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(SeasonId), nameof(RoleId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(EpisodeId), nameof(RoleId), IsUnique = true)]
    // [Index(nameof(Id),nameof(PersonId), IsUnique = true)]
    public class Cast
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("credit_id")] public string? CreditId { get; set; }

        [JsonProperty("person_id")] public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        [JsonProperty("role_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [JsonProperty("tv_id")] public int? TvId { get; set; }
        public virtual Tv Tv { get; set; }

        [JsonProperty("season_id")] public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }

        [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }

        [JsonProperty("role_id")] public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public Cast()
        {
        }

        public Cast(TMDBCast cast, TMDBMovie movieAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Role> roles)
        {
            CreditId = cast.CreditId;
            PersonId = cast.Id;
            MovieId = movieAppends.Id;
            RoleId = roles.Where(r => r.CreditId == cast.CreditId)?.FirstOrDefault()?.Id;
        }

        public Cast(TMDBCast cast, TMDBTv tvAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Role> roles)
        {
            CreditId = cast.CreditId;
            PersonId = cast.Id;
            TvId = tvAppends.Id;
            RoleId = roles.Where(r => r.CreditId == cast.CreditId)?.FirstOrDefault()?.Id;
        }

        public Cast(TMDBCast cast, TMDBSeason seasonAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Role> roles)
        {
            CreditId = cast.CreditId;
            PersonId = cast.Id;
            SeasonId = seasonAppends.Id;
            RoleId = roles.Where(r => r.CreditId == cast.CreditId)?.FirstOrDefault()?.Id;
        }

        public Cast(TMDBCast cast, TMDBEpisode episodeAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Role> roles)
        {
            CreditId = cast.CreditId;
            PersonId = cast.Id;
            EpisodeId = episodeAppends.Id;
            RoleId = roles.Where(r => r.CreditId == cast.CreditId)?.FirstOrDefault()?.Id;
        }
    }
}