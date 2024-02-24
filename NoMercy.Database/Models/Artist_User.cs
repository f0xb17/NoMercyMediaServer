using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(ArtistId), nameof(UserId))]
    public class ArtistUser
    {
        [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public ArtistUser()
        {
        }

        public ArtistUser(Guid artistId, Guid userId)
        {
            ArtistId = artistId;
            UserId = userId;
        }
    }
}