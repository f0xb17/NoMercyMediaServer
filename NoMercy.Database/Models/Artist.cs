using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Artist: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Folder { get; set; }
        public string? Cover { get; set; } 
        
        public required string LibraryId { get; set; }
        
        public virtual Library Library { get; } = null!;
    }
}