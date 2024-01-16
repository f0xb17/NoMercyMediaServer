using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(UserId))]
    public class Artist_User
    {
        public required string ArtistId { get; set; }
        public required string UserId { get; set; }

        public virtual Artist Artist { get; } = null!;
        public virtual User User { get; } = null!;        
    }
}