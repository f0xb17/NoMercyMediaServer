using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.Season;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Season: ColorPalettes
    {
        public Season(SeasonAppends s, int tvId)
        {
            Id = s.Id;
            Title = s.Name;
            AirDate = s.AirDate;
            EpisodeCount = s.Episodes.Count;
            Overview = s.Overview;
            Poster = s.PosterPath;
            SeasonNumber = s.SeasonNumber;
            TvId = tvId;
        }
        
        public Season()
        { }
        
	    [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? AirDate { get; set; }
        public int EpisodeCount { get; set; }
        public string? Overview { get; set; }
        public string? Poster { get; set; }
        public int SeasonNumber { get; set; }
        
        public int TvId { get; set; }
        public virtual Tv Tv { get; set; }
            
        public virtual ICollection<Episode> Episodes { get; set; } = new HashSet<Episode>();
        public virtual ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();
        public virtual ICollection<Crew> Crews { get; set; } = new HashSet<Crew>();
        public virtual ICollection<Media> Medias { get; set; } = new HashSet<Media>();
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<GuestStar> GuestStars { get; set; } = new HashSet<GuestStar>();
        public virtual ICollection<Translation> Translations { get; set; } = new HashSet<Translation>();
    }
}