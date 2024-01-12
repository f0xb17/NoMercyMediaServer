using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(MusicGenreId))]
    public class Artist_MusicGenre
    {
        public required string ArtistId { get; set; }
        public required string MusicGenreId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual MusicGenre MusicGenre { get; set; }        
    }
}