#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(MediaId), nameof(TvFromId), IsUnique = true)]
    [Index(nameof(MediaId), nameof(MovieFromId), IsUnique = true)]
    public class Similar : ColorPalettes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("backdrop")] public string? Backdrop { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }
        [JsonProperty("poster")] public string? Poster { get; set; }
        [JsonProperty("title")] public string? Title { get; set; }
        [JsonProperty("titleSort")] public string? TitleSort { get; set; }
        [JsonProperty("media_id")] public int MediaId { get; set; }

        [JsonProperty("tv_from_id")] public int? TvFromId { get; set; }
        public virtual Tv TvFrom { get; set; }

        [JsonProperty("tv_to_id")] public int? TvToId { get; set; }
        public virtual Tv TvTo { get; set; }

        [JsonProperty("movie_from_id")] public int? MovieFromId { get; set; }
        public virtual Movie MovieFrom { get; set; }

        [JsonProperty("movie_to_id")] public int? MovieToId { get; set; }
        public virtual Movie MovieTo { get; set; }

        public Similar()
        {
        }

        public Similar(SimilarTvShow similar, TvShowAppends show)
        {
            Backdrop = similar.BackdropPath;
            Overview = similar.Overview;
            Poster = similar.PosterPath;
            Title = similar.Name;
            TitleSort = similar.Name;
            MediaId = similar.Id;
            TvFromId = show.Id;
            // TvToId = similar.Id;
        }

        public Similar(SimilarMovie similar, MovieAppends movie)
        {
            Backdrop = similar.BackdropPath;
            Overview = similar.Overview;
            Poster = similar.PosterPath;
            Title = similar.Title;
            TitleSort = similar.Title;
            MediaId = similar.Id;
            MovieFromId = movie.Id;
            // TvToId = similar.Id;
        }
    }
}