using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class EncoderProfile: Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string Id { get; set; }
        [Key]
        public required string Name { get; set; }
        public string? Container { get; set; }
        public string? Param { get; set; }
        
        public virtual ICollection<EncoderProfile_Library> EncoderProfile_Library { get; } = new HashSet<EncoderProfile_Library>();
    }
}