using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(TrackId))]
    public class Artist_Track
    {
        public required string ArtistId { get; set; }
        public required string TrackId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Track Track { get; set; }        
    }
}