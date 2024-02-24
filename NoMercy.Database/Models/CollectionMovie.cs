using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CollectionId), nameof(MovieId))]
    public class CollectionMovie
    {
        [JsonProperty("collection_id")] public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [JsonProperty("movie_id")] public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public CollectionMovie()
        {
        }

        public CollectionMovie(int collectionId, int movieId)
        {
            CollectionId = collectionId;
            MovieId = movieId;
        }

        public CollectionMovie(Providers.TMDB.Models.Movies.Movie collectionId, int collectionsId)
        {
            MovieId = collectionId.Id;
            CollectionId = collectionsId;
        }
    }
}