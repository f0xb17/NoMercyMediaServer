using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(UserId))]
    public class CollectionUser
    {
        [JsonProperty("collection_id")] public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public CollectionUser()
        {
        }

        public CollectionUser(int collectionId, Guid userId)
        {
            CollectionId = collectionId;
            UserId = userId;
        }
    }
}