#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Title), nameof(TvId), IsUnique = true)]
    [Index(nameof(Title), nameof(MovieId), IsUnique = true)]
    public class AlternativeTitle
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("iso_3166_1")] public string? Iso31661 { get; set; }

        [JsonProperty("title")] public string? Title { get; set; }

        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [JsonProperty("tv_id")] public int? TvId { get; set; }
        public virtual Tv Tv { get; set; }

        public AlternativeTitle(TvAlternativeTitle tvAlternativeTitles, int tvId)
        {
            Iso31661 = tvAlternativeTitles.Iso31661;
            Title = tvAlternativeTitles.Title;
            TvId = tvId;
        }

        public AlternativeTitle()
        {
        }

        public AlternativeTitle(MovieAlternativeTitle movieAlternativeTitles, int movieId)
        {
            Iso31661 = movieAlternativeTitles.Iso31661;
            Title = movieAlternativeTitles.Title;
            MovieId = movieId;
        }
    }
}