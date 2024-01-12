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
        public string Iso6391 { get; set; }
        public string EnglishName { get; set; }
        public string? Name { get; set; }
        
        public virtual ICollection<Language_Library> Language_Library { get; set; } = new HashSet<Language_Library>();
        
    }
}