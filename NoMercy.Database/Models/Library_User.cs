using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(UserId))]
    public class Library_User
    {
        public required string LibraryId { get; set; }
        public required string UserId { get; set; }

        public virtual Library Library { get; set; }
        public virtual User User { get; set; }        
    }
}