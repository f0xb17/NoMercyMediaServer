using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(TvId))]
    public class Library_Tv
    {
        public required string LibraryId { get; set; }
        public required int TvId { get; set; }

        public virtual Library Library { get; } = null!;
        public virtual Tv Tv { get; }        
    }
}