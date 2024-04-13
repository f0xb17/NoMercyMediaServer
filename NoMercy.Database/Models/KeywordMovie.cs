#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(KeywordId), nameof(MovieId))]
    public class KeywordMovie
    {
        [JsonProperty("keyword_id")] public int KeywordId { get; set; }
        public virtual Keyword Keyword { get; set; }
        
        [JsonProperty("movie_id")] public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public KeywordMovie()
        {
        }

        public KeywordMovie(Providers.TMDB.Models.Shared.Keyword keyword, MovieAppends movie)
        {
            KeywordId = keyword.Id;
            MovieId = movie.Id;
        }
    }
}