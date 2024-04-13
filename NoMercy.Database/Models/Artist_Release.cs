#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(ReleaseId))]
    public class ArtistRelease
    {
        [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        
        [JsonProperty("release_id")] public Guid ReleaseId { get; set; }
        public virtual Release Release { get; set; }
        
        public ArtistRelease()
        {
        }
        
        public ArtistRelease(Guid albumId, Guid releaseId)
        {
            ArtistId = albumId;
            ReleaseId = releaseId;
        }
    }
}