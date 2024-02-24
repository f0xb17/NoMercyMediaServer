using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Artist : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")] public Guid Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string? Description { get; set; }

        [JsonProperty("folder")] public string Folder { get; set; }

        [JsonProperty("cover")] public string? Cover { get; set; }

        [JsonProperty("library_id")] public required Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public Artist()
        {
        }

        public Artist(string name, string folder, Ulid libraryId)
        {
            Name = name;
            Folder = folder;
            LibraryId = libraryId;
        }
    }
}