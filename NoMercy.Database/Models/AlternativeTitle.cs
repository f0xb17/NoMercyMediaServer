using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Title), IsUnique = true)]
    public class AlternativeTitle
    {
        public AlternativeTitle(TvAlternativeTitle tvAlternativeTitles)
        {
            Iso31661 = tvAlternativeTitles.Iso31661;
            Title = tvAlternativeTitles.Title;
        }

        public AlternativeTitle()
        { }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Iso31661 { get; set; }
        public string Title { get; set; }
        
        public int? MovieId { get; set; }
        public int? TvId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Tv Tv { get; set; }
        
    }
}