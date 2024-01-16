using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(MovieId))]
    public class Library_Movie
    {
        public required string LibraryId { get; set; }
        public required int MovieId { get; set; }

        public virtual Library Library { get; } = null!;
        public virtual Movie Movie { get; }        
    }
}