using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class ActivityLog: Timestamps
    {
        public int Id { get; private set; }

        [StringLength(255)]
        public required string Type { get; set; }
        [StringLength(36)]
        public required string UserId { get; set; }
        [StringLength(36)]
        public required string DeviceId { get; set; }
        
        public required DateTime Time { get; set; }
        
        public virtual Device Device { get; set; }
        public virtual User User { get; set; }
    }
}