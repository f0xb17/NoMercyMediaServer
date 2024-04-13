#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(NotificationId), nameof(UserId))]
    public class NotificationUser
    {
        [JsonProperty("notification_id")] public Ulid NotificationId { get; set; }
        public virtual Notification Notification { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public NotificationUser()
        {
        }

        public NotificationUser(Ulid notificationId, Guid userId)
        {
            NotificationId = notificationId;
            UserId = userId;
        }
    }
}