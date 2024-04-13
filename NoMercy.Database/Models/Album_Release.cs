#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(ReleaseId))]
    public class AlbumRelease
    {
        [JsonProperty("album_id")] public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
        
        [JsonProperty("release_id")] public Guid ReleaseId { get; set; }
        public virtual Release Release { get; set; }
        
        public AlbumRelease()
        {
        }
        public AlbumRelease(Guid albumId, Guid releaseId)
        {
            AlbumId = albumId;
            ReleaseId = releaseId;
        }
    }
}