#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(GenreId), nameof(TvId))]
    public class GenreTv
    {
        [JsonProperty("genre_id")] public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }

        public GenreTv()
        {
        }

        public GenreTv(Providers.TMDB.Models.Shared.Genre genre, TvShowAppends tv)
        {
            GenreId = genre.Id;
            TvId = tv.Id;
        }
    }
}