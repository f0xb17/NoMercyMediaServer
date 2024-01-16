using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(GenreId), nameof(TrackId))]
    public class MusicGenre_Track
    {
        public required string GenreId { get; set; }
        public required string TrackId { get; set; }

        public virtual Genre Genre { get; } = null!;
        public virtual Track Track { get; }        
    }
}