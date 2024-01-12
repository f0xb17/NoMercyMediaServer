using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Device: Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string Id { get; set; }
        public required string DeviceId { get; set; }
        public required string Browser { get; set; }
        public required string Os { get; set; }
        [Column("Device")]
        [StringLength(255)]
        public required string Model { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
        public string? CustomName { get; set; }
        public required string Version { get; set; }
        public required string Ip { get; set; }
    }
}