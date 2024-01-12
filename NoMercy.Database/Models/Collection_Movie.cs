using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(MovieId))]
    public class Collection_Movie
    {
        public required string CollectionId { get; set; }
        public required int MovieId { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Movie Movie { get; set; }        
    }
}