using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Id),nameof(MovieId), IsUnique = true)]
    [Index(nameof(Id),nameof(TvId), IsUnique = true)]
    [Index(nameof(Id),nameof(SeasonId), IsUnique = true)]
    [Index(nameof(Id),nameof(EpisodeId), IsUnique = true)]
    public class Crew
    {
        public Crew(Providers.TMDB.Models.Shared.Crew crew, Providers.TMDB.Models.Movies.Movie movieAppends)
        {
            Id = crew.CreditId;
            PersonId = crew.Id;
            MovieId = movieAppends.Id;
        }
        public Crew(Providers.TMDB.Models.Shared.Crew crew, Providers.TMDB.Models.TV.TvShow tvAppends)
        {
            Id = crew.CreditId;
            PersonId = crew.Id;
            TvId = tvAppends.Id;
        }
        public Crew(Providers.TMDB.Models.Shared.Crew crew, Providers.TMDB.Models.Season.Season seasonAppends)
        {
            Id = crew.CreditId;
            PersonId = crew.Id;
            SeasonId = seasonAppends.Id;
        }
        public Crew(Providers.TMDB.Models.Shared.Crew crew, Providers.TMDB.Models.Episode.Episode episodeAppends)
        {
            Id = crew.CreditId;
            PersonId = crew.Id;
            EpisodeId = episodeAppends.Id;
        }
        
        public Crew()
        {
        }
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public int? PersonId { get; set; }
        public int? MovieId { get; set; }
        public int? TvId { get; set; }
        public int? SeasonId { get; set; }
        public int? EpisodeId { get; set; }
        
        public virtual ICollection<Person>? People { get; set; } = new HashSet<Person>();
        
        public virtual Movie Movie { get; set; }
        public virtual Tv Tv { get; set; }
        public virtual Season Season { get; set; }
        public virtual Episode Episode { get; set; }
    }
}