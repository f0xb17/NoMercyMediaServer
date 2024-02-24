using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(TrackId))]
    public class AlbumTrack
    {
        [JsonProperty("album_id")] public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
        
        [JsonProperty("track_id")] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        public AlbumTrack()
        {
        }

        public AlbumTrack(Guid albumId, Guid trackId)
        {
            AlbumId = albumId;
            TrackId = trackId;
        }
    }
}