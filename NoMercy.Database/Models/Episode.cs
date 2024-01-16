using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.Episode;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Episode: ColorPalettes
    {
        public Episode(EpisodeAppends e, int tvId, int seasonId)
        {
            Id = e.Id;
            Title = e.Name;
            AirDate = e.AirDate;
            EpisodeNumber = e.EpisodeNumber;
            ImdbId = e.ExternalIds?.ImdbId;
            Overview = e.Overview;
            ProductionCode = e.ProductionCode;
            SeasonNumber = e.SeasonNumber;
            Still = e.StillPath;
            TvdbId = e.ExternalIds?.TvdbId;
            VoteAverage = e.VoteAverage;
            VoteCount = e.VoteCount;
            
            TvId = tvId;
            SeasonId = seasonId;
        }
        
        public Episode()
        { }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? AirDate { get; set; }
        public int EpisodeNumber { get; set; }
        public string? ImdbId { get; set; }
        public string? Overview { get; set; }
        public string? ProductionCode { get; set; }
        public int SeasonNumber { get; set; }
        public string? Still { get; set; }
        public int? TvdbId { get; set; }
        public float? VoteAverage { get; set; }
        public int? VoteCount { get; set; }
        
        public int TvId { get; set; }
        public int SeasonId { get; set; }

        public virtual Tv Tv { get; } = null!;
        public virtual Season Season { get; set; } = null!;
        
        public virtual ICollection<Cast>? Casts { get; set; } = null!;
        public virtual ICollection<Crew>? Crews { get; set; } = null!;
        public virtual ICollection<SpecialItem>? SpecialItems { get; set; } = null!;
        public virtual ICollection<VideoFile>? VideoFiles { get; set; } = null!;
        public virtual ICollection<Media>? Medias { get; set; } = null!;
        public virtual ICollection<Image>? Images { get; set; } = null!;
        public virtual ICollection<GuestStar>? GuestStars { get; set; } = null!;
        public virtual ICollection<File>? Files { get; set; } = null!;
        public virtual ICollection<Translation>? Translations { get; set; } = null!;
    }
}