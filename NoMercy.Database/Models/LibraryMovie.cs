#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(LibraryId), nameof(MovieId))]
    public class LibraryMovie
    {
        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        [JsonProperty("movie_id")] public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public LibraryMovie()
        {
        }

        public LibraryMovie(Ulid libraryId, int movieId)
        {
            LibraryId = libraryId;
            MovieId = movieId;
        }
    }
}