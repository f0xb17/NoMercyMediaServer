using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(NotificationId), nameof(UserId))]
    public class Notification_User
    {
        public required string NotificationId { get; set; }
        public required string UserId { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual User User { get; set; }        
    }
}