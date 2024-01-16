using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(MusicGenreId))]
    public class Album_MusicGenre
    {
        public required string AlbumId { get; set; }
        public required string MusicGenreId { get; set; }

        public virtual Album Album { get; } = null!;
        public virtual MusicGenre MusicGenre { get; }        
    }
}