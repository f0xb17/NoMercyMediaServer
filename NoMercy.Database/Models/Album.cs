using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Album: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")] public Guid Id { get; set; }
        
        [StringLength(255)]
        [JsonProperty("name")] public required string Name { get; set; }
        
        [StringLength(255)]
        [JsonProperty("description")] public string? Description { get; set; }
        
        [StringLength(255)]
        [JsonProperty("folder")] public required string Folder { get; set; }
        
        [StringLength(255)] 
        [JsonProperty("cover")] public string? Cover { get; set; }
        
        [StringLength(2)]
        [JsonProperty("country")] public required string Country { get; set; }
        
        [StringLength(4)]
        [JsonProperty("year")] public required int Year { get; set; }
        
        [JsonProperty("genre")] public required int Tracks { get; set; }
        
        public required Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

    }
}