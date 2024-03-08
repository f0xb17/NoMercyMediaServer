using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(TvId), nameof(UserId))]
    public class TvUser
    {
        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public TvUser()
        {
        }

        public TvUser(int tvId, Guid userId)
        {
            TvId = tvId;
            UserId = userId;
        }
    }
}