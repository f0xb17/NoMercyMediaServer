using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(UserId))]
    public class Album_User
    {
        public required string AlbumId { get; set; }
        public required string UserId { get; set; }

        public virtual Album Album { get; set; }
        public virtual User User { get; set; }        
    }
}