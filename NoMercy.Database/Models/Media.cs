using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(TvId), nameof(Src), IsUnique = true)]
    [Index(nameof(SeasonId), nameof(Src), IsUnique = true)]
    [Index(nameof(EpisodeId), nameof(Src), IsUnique = true)]
    [Index(nameof(MovieId), nameof(Src), IsUnique = true)]
    [Index(nameof(PersonId), nameof(Src), IsUnique = true)]
    [Index(nameof(VideoFileId), nameof(Src), IsUnique = true)]
    public class Media : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        [JsonProperty("iso_639_1")] public string? Iso6391 { get; set; }

        [JsonProperty("name")] public string? Name { get; set; }

        [JsonProperty("site")] public string? Site { get; set; }

        [JsonProperty("size")] public int Size { get; set; }

        [JsonProperty("src")] public string Src { get; set; }

        [JsonProperty("type")] public string? Type { get; set; }
        
        [JsonProperty("tv_id")] public int? TvId { get; set; }
        public virtual Tv Tv { get; set; }
        
        [JsonProperty("season_id")] public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }
        
        [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }
        
        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        
        [JsonProperty("person_id")] public int? PersonId { get; set; }
        public virtual Person Person { get; set; }

        [JsonProperty("video_file_id")] public Ulid? VideoFileId { get; set; }
        public virtual VideoFile VideoFile { get; set; }

        public Media()
        {
        }

        public Media(TvVideo media, TvShow show, string type)
        {
            Id = Ulid.NewUlid();
            Iso6391 = media.Iso6391;
            Name = media.Name;
            Site = media.Site;
            Size = media.Size;
            Src = media.Key;
            Type = type;
            TvId = show.Id;
        }

        public Media(MovieVideo media, MovieAppends movie, string type)
        {
            Id = Ulid.NewUlid();
            Iso6391 = media.Iso6391;
            Name = media.Name;
            Site = media.Site;
            Size = media.Size;
            Src = media.Key;
            Type = type;
            MovieId = movie.Id;
        }
    }
}