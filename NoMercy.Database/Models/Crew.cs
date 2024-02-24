using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TMDBCrew = NoMercy.Providers.TMDB.Models.Shared.Crew;
using TMDBMovie = NoMercy.Providers.TMDB.Models.Movies.Movie;
using TMDBTv = NoMercy.Providers.TMDB.Models.TV.TvShowAppends;
using TMDBSeason = NoMercy.Providers.TMDB.Models.Season.Season;
using TMDBEpisode = NoMercy.Providers.TMDB.Models.Episode.Episode;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(CreditId), nameof(MovieId), nameof(JobId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(TvId), nameof(JobId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(SeasonId), nameof(JobId), IsUnique = true)]
    [Index(nameof(CreditId), nameof(EpisodeId), nameof(JobId), IsUnique = true)]
    public class Crew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("credit_id")] public string CreditId { get; set; }

        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        
        [JsonProperty("tv_id")] public int? TvId { get; set; }
        public virtual Tv Tv { get; set; }
        
        [JsonProperty("season_id")] public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }
        
        [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }

        [JsonProperty("person_id")] public int? PersonId { get; set; }
        public virtual Person Person { get; set; }

        [JsonProperty("job_id")] public int? JobId { get; set; }
        public virtual Job? Job { get; set; }

        public Crew()
        {
        }

        public Crew(TMDBCrew crew, TMDBMovie movieAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Job> jobs)
        {
            CreditId = crew.CreditId;
            PersonId = crew.Id;
            MovieId = movieAppends.Id;
            JobId = jobs.Where(r => r.CreditId == crew.CreditId)?.FirstOrDefault()?.Id;
        }

        public Crew(TMDBCrew crew, TMDBTv tvAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Job> jobs)
        {
            CreditId = crew.CreditId;
            PersonId = crew.Id;
            TvId = tvAppends.Id;
            JobId = jobs.Where(r => r.CreditId == crew.CreditId)?.FirstOrDefault()?.Id;
        }

        public Crew(TMDBCrew crew, TMDBSeason seasonAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Job> jobs)
        {
            CreditId = crew.CreditId;
            PersonId = crew.Id;
            SeasonId = seasonAppends.Id;
            JobId = jobs.Where(r => r.CreditId == crew.CreditId)?.FirstOrDefault()?.Id;
        }

        public Crew(TMDBCrew crew, TMDBEpisode episodeAppends, TMDBMovie movie, TMDBTv tv, TMDBSeason season,
            IEnumerable<Job> jobs)
        {
            CreditId = crew.CreditId;
            PersonId = crew.Id;
            EpisodeId = episodeAppends.Id;
            JobId = jobs.Where(r => r.CreditId == crew.CreditId)?.FirstOrDefault()?.Id;
        }
    }
}