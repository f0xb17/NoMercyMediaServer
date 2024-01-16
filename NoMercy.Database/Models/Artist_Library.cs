using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey("ArtistId", "LibraryId")]
    public class Artist_Library
    {
        public required string ArtistId { get; set; }
        public required string LibraryId { get; set; }

        public virtual Artist Artist { get; } = null!;
        public virtual Library Library { get; } = null!;        
    }
}