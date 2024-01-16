using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(PlaylistId), nameof(TrackId))]
    public class Playlist_Track
    {
        public required string PlaylistId { get; set; }
        public required string TrackId { get; set; }

        public virtual Playlist Playlist { get; } = null!;
        public virtual Track Track { get; set; }        
    }
}