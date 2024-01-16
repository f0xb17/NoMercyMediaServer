using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Id), IsUnique = true)]
    public class Library: Timestamps, IDisposable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public int AutoRefreshInterval { get; set; }
        public bool ChapterImages { get; set; }
        public bool ExtractChapters { get; set; }
        public bool ExtractChaptersDuring { get; set; }
        public string? Image { get; set; }
        public bool PerfectSubtitleMatch { get; set; }
        public bool Realtime { get; set; }
        public string? SpecialSeasonName { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Country { get; set; }
        public string? Language { get; set; }
            
        public virtual ICollection<Folder_Library> Folder_Libraries { get; } = new HashSet<Folder_Library>();
        public virtual ICollection<Language_Library> Language_Libraries { get; } = new HashSet<Language_Library>();
        public virtual ICollection<EncoderProfile_Library> EncoderProfile_Libraries { get; } = new HashSet<EncoderProfile_Library>();
        public virtual ICollection<Library_User> Library_Users { get; } = new HashSet<Library_User>();
        public virtual ICollection<File_Library> File_Libraries { get; set; } = new HashSet<File_Library>();
        
        public virtual ICollection<Library_Tv> Library_Tvs { get; set; } = new HashSet<Library_Tv>();
        public virtual ICollection<Library_Movie> Library_Movies { get; set; } = new HashSet<Library_Movie>();
        public virtual ICollection<Library_Track> Library_Tracks { get; set; } = new HashSet<Library_Track>();
        
        public virtual ICollection<Collection_Library> Collection_Libraries { get; set; } = new HashSet<Collection_Library>();
        public virtual ICollection<Album_Library> Album_Libraries { get; set; } = new HashSet<Album_Library>();
        public virtual ICollection<Artist_Library> Artist_Libraries { get; set; } = new HashSet<Artist_Library>();
        
        public Library()
        {
            
        }
        
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}