using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(KeywordId), nameof(TvId))]
    public class Keyword_Tv
    {
        public required string KeywordId { get; set; }
        public required int TvId { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Tv Tv { get; set; }        
    }
}