using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models;
    
[PrimaryKey(nameof(Id))]
public class Job
{ 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [MaxLength(255)]
    public string Queue { get; set; }
    [MaxLength(4092)]
    public required string Payload { get; set; }
    public byte Attempts { get; set; } = 0;
    public DateTime? ReservedAt { get; set; }
    public DateTime AvailableAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

