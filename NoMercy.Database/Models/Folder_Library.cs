using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{    
    [PrimaryKey(nameof(FolderId), nameof(LibraryId))]
    public class Folder_Library
    {
        public required string FolderId { get; set; }
        public required string LibraryId { get; set; }

        public virtual Folder Folder { get; set; }
        public virtual Library Library { get; set; }        
    }
}