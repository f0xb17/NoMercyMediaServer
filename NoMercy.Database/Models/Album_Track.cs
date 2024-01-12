using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(TrackId))]
    public class Album_Track
    {
        public required string AlbumId { get; set; }
        public required string TrackId { get; set; }

        public virtual Album Album { get; set; }
        public virtual Track Track { get; set; }        
    }
}