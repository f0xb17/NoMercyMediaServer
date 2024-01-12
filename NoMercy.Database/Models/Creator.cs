using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(PersonId), nameof(TvId))]
    public class Creator
    {
        public required int PersonId { get; set; }
        public required int TvId { get; set; }
        
        public virtual Person Person { get; set; }
        public virtual Tv Tv { get; set; }
        
    }
}