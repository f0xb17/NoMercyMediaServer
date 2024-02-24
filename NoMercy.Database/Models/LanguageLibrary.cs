using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LanguageId), nameof(LibraryId))]
    public class LanguageLibrary
    {
        [JsonProperty("language_id")] public int LanguageId { get; set; }
        public virtual Language Language { get; set; }


        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public LanguageLibrary()
        {
        }

        public LanguageLibrary(int languageId, Ulid libraryId)
        {
            LanguageId = languageId;
            LibraryId = libraryId;
        }
    }
}