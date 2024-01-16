using Microsoft.EntityFrameworkCore;

// ReSharper disable once InconsistentNaming
namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(UserId), nameof(TrackId))]
    public class Music_Play
    {
        public required string UserId { get; set; }
        public required string TrackId { get; set; }

        public virtual User User { get; } = null!;
        public virtual Track Track { get; set; }        
    }
}