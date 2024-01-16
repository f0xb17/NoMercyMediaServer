using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Movie: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string TitleSort { get; set; }
        public string? Duration { get; set; }
        public bool Show { get; set; }
        public string? Folder { get; set; }
        public bool Adult { get; set; }
        public string? Backdrop { get; set; }
        public int? Budget { get; set; }
        public string? Homepage { get; set; }
        public string? ImdbId { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? Overview { get; set; }
        public double? Popularity { get; set; }
        public string? Poster { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Revenue { get; set; }
        public int? Runtime { get; set; }
        public string? Status { get; set; }
        public string? Tagline { get; set; }
        public string? Trailer { get; set; }
        public int? MoviedbId { get; set; }
        public string? Video { get; set; }
        public double? VoteAverage { get; set; }
        public int? VoteCount { get; set; }
        public string? BlurHash { get; set; }
        
        public string? LibraryId { get; set; }
        
        public virtual Library Library { get; } = null!;
        public virtual ICollection<AlternativeTitle>? AlternativeTitles { get; } = new HashSet<AlternativeTitle>();
        public virtual ICollection<Cast>? Cast { get; } = new HashSet<Cast>();
        public virtual ICollection<Certification_Movie>? Certification_Movies { get; } = new HashSet<Certification_Movie>();
        public virtual ICollection<Crew>? Crew { get; } = new HashSet<Crew>();
        public virtual ICollection<Genre_Movie>? Genre_Movies { get; } = new HashSet<Genre_Movie>();
        public virtual ICollection<Keyword_Movie>? Keyword_Movies { get; } = new HashSet<Keyword_Movie>();
        public virtual ICollection<Media>? Media { get; } = new HashSet<Media>();
        public virtual ICollection<Image>? Images { get; } = new HashSet<Image>();
        public virtual ICollection<Recommendation>? Recommendation_From { get; } = new HashSet<Recommendation>();
        public virtual ICollection<Recommendation>? Recommendation_To { get; }	= new HashSet<Recommendation>();
        public virtual ICollection<Season>? Seasons { get; } = new HashSet<Season>();
		    
        public virtual ICollection<Similar>? Similar_From { get; } = new HashSet<Similar>();
        public virtual ICollection<Similar>? Similar_To { get; } = new HashSet<Similar>();
        public virtual ICollection<Translation>? Translations { get; set; } = new HashSet<Translation>();
        public virtual ICollection<UserData>? UserData { get; set; } = new HashSet<UserData>();
    
        
    }
}