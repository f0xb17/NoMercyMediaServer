using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(GenreId), nameof(TvId))]
    public class Genre_Tv
    {
        public required string GenreId { get; set; }
        public required int TvId { get; set; }

        public virtual Genre Genre { get; } = null!;
        public virtual Tv Tv { get; }        
    }
}