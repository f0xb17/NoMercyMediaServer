#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(GenreId), nameof(MovieId))]
public class GenreMovie
{
    [JsonProperty("genre_id")] public int GenreId { get; set; }
    public Genre Genre { get; set; }

    [JsonProperty("movie_id")] public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public GenreMovie()
    {
    }

    public GenreMovie(Providers.TMDB.Models.Shared.TmdbGenre tmdbGenre, TmdbMovieAppends tmdbMovie)
    {
        GenreId = tmdbGenre.Id;
        MovieId = tmdbMovie.Id;
    }
}