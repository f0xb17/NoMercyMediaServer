using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(KeywordId), nameof(MovieId))]
    public class Keyword_Movie
    {
        public required int KeywordId { get; set; }
        public required int MovieId { get; set; }

        public virtual Keyword Keyword { get; } = null!;
        public virtual Movie Movie { get; } = null!;        
    }
}