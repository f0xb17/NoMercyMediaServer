using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey("ArtistId", "LibraryId")]
    public class ArtistLibrary
    {
        [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public ArtistLibrary()
        {
        }

        public ArtistLibrary(Guid artistId, Ulid libraryId)
        {
            ArtistId = artistId;
            LibraryId = libraryId;
        }
    }
}