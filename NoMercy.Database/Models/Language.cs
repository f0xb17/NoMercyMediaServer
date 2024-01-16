using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{    
    [Index(nameof(Iso6391), IsUnique = true)]
    [PrimaryKey(nameof(Id))]
    public class Language
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Iso6391 { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string? Name { get; set; }
        
        public virtual ICollection<Language_Library> Language_Library { get; } = new HashSet<Language_Library>();
        
        public Language()
        {
            
        }
        
        public Language(Providers.TMDB.Models.Configuration.Language language)
        {
            Iso6391 = language.Iso6391;
            EnglishName = language.EnglishName;
            Name = language.Name;
        }

    }
}