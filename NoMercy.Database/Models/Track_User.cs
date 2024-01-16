// ReSharper disable InconsistentNaming
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(TrackId), nameof(UserId))]
    public class Track_User
    {
        public required string TrackId { get; set; }
        public required string UserId { get; set; }

        public virtual Track Track { get; } = null!;
        public virtual User User { get; set; }        
    }
}