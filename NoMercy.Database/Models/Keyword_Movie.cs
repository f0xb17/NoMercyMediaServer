using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(KeywordId), nameof(MovieId))]
    public class Keyword_Movie
    {
        public required string KeywordId { get; set; }
        public required int MovieId { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Movie Movie { get; set; }        
    }
}