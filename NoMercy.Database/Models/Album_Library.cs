using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(LibraryId))]
    public class Album_Library
    {
        public required string AlbumId { get; set; }
        public required string LibraryId { get; set; }

        public virtual Album Album { get; } = null!;
        public virtual Library Library { get; } = null!;
        
    }
}