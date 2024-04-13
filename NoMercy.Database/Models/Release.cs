using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Release: ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")] 
    public Guid Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    public virtual Library Library { get; set; }
    
    [JsonProperty("albums")]
    public virtual ICollection<AlbumRelease> Albums { get; set; }
        
    [JsonProperty("artists")]
    public virtual ICollection<ArtistRelease> Artists { get; set; }

}