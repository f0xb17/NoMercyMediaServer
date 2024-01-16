using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(FileId), nameof(MovieId))]
    public class File_Movie
    {
        public required string FileId { get; set; }
        public required int MovieId { get; set; }

        public virtual File File { get; } = null!;
        public virtual Movie Movie { get; }        
    }
}