using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(GenreId), nameof(MovieId))]
   public class Genre_Movie
    {
        public required string GenreId { get; set; }
        public required int MovieId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Movie Movie { get; set; }        
    }
}