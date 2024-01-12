using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(ArtistId))]
    public class Album_Artist
    {
        public required string AlbumId { get; set; }
        public required string ArtistId { get; set; }

        public virtual Album Album { get; set; }
        public virtual Artist Artist { get; set; }

    }
}