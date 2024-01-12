using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LanguageId), nameof(LibraryId))]
    public class Language_Library
    {
        public required string LanguageId { get; set; }
        public required string LibraryId { get; set; }

        public virtual Language Language { get; set; }
        public virtual Library Library { get; set; }        
    }
}