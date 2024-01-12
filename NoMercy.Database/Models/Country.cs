using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{    
    [Index(nameof(Iso31661), IsUnique = true)]
    [PrimaryKey(nameof(Id))]
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string Iso31661 { get; set; }
        public string? EnglishName { get; set; }
        public string? NativeName { get; set; }
    }
}