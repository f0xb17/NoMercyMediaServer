using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(MovieId))]
    public class Collection_Movie
    {
        public required int CollectionId { get; set; }
        public required int MovieId { get; set; }

        public virtual Collection Collection { get; } = null!;
        public virtual Movie Movie { get; set; }        
    }
}