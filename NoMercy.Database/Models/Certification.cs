using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Iso31661), nameof(Rating), IsUnique = true)]
    public class Certification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Iso31661 { get; set; }
        public string Rating { get; set; }
        public string Meaning { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}