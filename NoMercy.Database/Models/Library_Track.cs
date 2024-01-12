using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(TrackId))]
    public class Library_Track
    {
        public required string LibraryId { get; set; }
        public required string TrackId { get; set; }

        public virtual Library Library { get; set; }
        public virtual Track Track { get; set; }        
    }
}