#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Album: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")] 
        public Guid Id { get; set; }
        
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("description")] public string? Description { get; set; }
        [JsonProperty("cover")] public string? Cover { get; set; }
        [JsonProperty("country")] public string? Country { get; set; }
        [JsonProperty("year")] public int Year { get; set; }
        [JsonProperty("tracks")] public int Tracks { get; set; }
        
        [JsonProperty("folder")] public string? Folder { get; set; } 
        [JsonProperty("host_folder")] public string? HostFolder { get; set; }
        
        [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
        public virtual Library Library { get; set; }
        
        [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
        public virtual Folder LibraryFolder { get; set; }
        
        [JsonProperty("album_track")]
        public virtual ICollection<AlbumTrack> AlbumTrack { get; set; }
        
        [JsonProperty("album_artist")]
        public virtual ICollection<AlbumArtist> AlbumArtist { get; set; }
        
        [JsonProperty("album_user")]
        public virtual ICollection<AlbumUser> AlbumUser { get; set; }
    }
}