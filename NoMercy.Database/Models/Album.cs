using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Album: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required string Id { get; set; }
        [StringLength(255)]
        public required string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [StringLength(255)]
        public required string Folder { get; set; }
        [StringLength(255)] 
        public string? Cover { get; set; }
        [StringLength(2)]
        public required string Country { get; set; }
        public required int Year { get; set; }
        public required int Tracks { get; set; }
        
        public required string LibraryId { get; set; }
        
        public virtual Library Library { get; set; }

    }
}