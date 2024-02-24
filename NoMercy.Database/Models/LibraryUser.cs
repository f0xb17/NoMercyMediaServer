using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(UserId))]
    public class LibraryUser
    {
        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public LibraryUser()
        {
        }

        public LibraryUser(Ulid libraryId, Guid userId)
        {
            LibraryId = libraryId;
            UserId = userId;
        }
    }
}