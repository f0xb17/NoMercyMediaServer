#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(GenreId), nameof(TvId))]
public class GenreTv
{
    [JsonProperty("genre_id")] public int GenreId { get; set; }
    public Genre Genre { get; set; }

    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    public GenreTv()
    {
    }

    public GenreTv(Providers.TMDB.Models.Shared.TmdbGenre tmdbGenre, TmdbTvShowAppends tmdbTv)
    {
        GenreId = tmdbGenre.Id;
        TvId = tmdbTv.Id;
    }
}