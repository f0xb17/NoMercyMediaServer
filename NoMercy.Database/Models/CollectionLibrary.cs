using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(LibraryId))]
    public class CollectionLibrary
    {
        [JsonProperty("collection_id")] public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        public CollectionLibrary()
        {
        }

        public CollectionLibrary(int collectionId, Ulid libraryId)
        {
            CollectionId = collectionId;
            LibraryId = libraryId;
        }
    }
}