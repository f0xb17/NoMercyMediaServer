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
    public class Cast
    {
        public Cast(Providers.TMDB.Models.Shared.Cast cast, Providers.TMDB.Models.Movies.Movie movieAppends)
        {
            Id = cast.CreditId;
            PersonId = cast.Id;
            MovieId = movieAppends.Id;
        }
        public Cast(Providers.TMDB.Models.Shared.Cast cast, Providers.TMDB.Models.TV.TvShow tvAppends)
        {
            Id = cast.CreditId;
            PersonId = cast.Id;
            TvId = tvAppends.Id;
        }
        public Cast(Providers.TMDB.Models.Shared.Cast cast, Providers.TMDB.Models.Season.Season seasonAppends)
        {
            Id = cast.CreditId;
            PersonId = cast.Id;
            SeasonId = seasonAppends.Id;
        }
        public Cast(Providers.TMDB.Models.Shared.Cast cast, Providers.TMDB.Models.Episode.Episode episodeAppends)
        {
            Id = cast.CreditId;
            PersonId = cast.Id;
            EpisodeId = episodeAppends.Id;
        }
        
        public Cast()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public int? PersonId { get; set; }
        public int? MovieId { get; set; }
        public int? TvId { get; set; }
        public int? SeasonId { get; set; }
        public int? EpisodeId { get; set; }
        
        public virtual ICollection<Person>? People { get; } = new HashSet<Person>();

        public virtual Movie? Movie { get; set; }
        public virtual Tv? Tv { get; set; }
        public virtual Season? Season { get; set; }
        public virtual Episode? Episode { get; set; }
        
    }
}