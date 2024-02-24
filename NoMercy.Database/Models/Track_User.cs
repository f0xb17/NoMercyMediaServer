// ReSharper disable InconsistentNaming

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(TrackId), nameof(UserId))]
    public class TrackUser
    {
        [JsonProperty("track_id")] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public TrackUser()
        {
        }

        public TrackUser(Guid trackId, Guid userId)
        {
            TrackId = trackId;
            UserId = userId;
        }
    }
}