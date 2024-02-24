using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// ReSharper disable once InconsistentNaming
namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(UserId), nameof(TrackId))]
    public class MusicPlay
    {
        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [JsonProperty("track_id")] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        public MusicPlay()
        {
        }

        public MusicPlay(Guid userId, Guid trackId)
        {
            UserId = userId;
            TrackId = trackId;
        }
    }
}