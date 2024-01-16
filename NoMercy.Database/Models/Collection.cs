using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Collection: ColorPalettes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public required string Title { get; set; }
        public required string TitleSort { get; set; }
        public string? Backdrop { get; set; }
        public string? Poster { get; set; }
        public string? Overview { get; set; }
        public int Parts { get; set; }

        public required string LibraryId { get; set; }
        
        public virtual Library Library { get; } = null!;
        
        
    }
}