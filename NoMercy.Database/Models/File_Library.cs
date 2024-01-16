using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(FileId), nameof(LibraryId))]
    public class File_Library
    {
        public required string FileId { get; set; }
        public required string LibraryId { get; set; }

        public virtual File File { get; } = null!;
        public virtual Library Library { get; }        
    }
}