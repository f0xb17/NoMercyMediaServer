using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(LibraryId))]
    public class Collection_Library
    {
        public required int CollectionId { get; set; }
        public required string LibraryId { get; set; }

        public virtual Collection Collection { get; } = null!;
        public virtual Library Library { get; } = null!;        
    }
}