using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Season;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Season : ColorPalettes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")] public string? Title { get; set; }
        [JsonProperty("air_date")] public DateTime? AirDate { get; set; }
        [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }
        [JsonProperty("poster_path")] public string? Poster { get; set; }
        [JsonProperty("season_number")] public int SeasonNumber { get; set; }

        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }

        [JsonProperty("episodes")]
        public virtual ICollection<Episode> Episodes { get; set; }
        
        [JsonProperty("casts")]
        public virtual ICollection<Cast> Casts { get; set; }
        
        [JsonProperty("crews")]
        public virtual ICollection<Crew> Crews { get; set; }
        
        [JsonProperty("medias")]
        public virtual ICollection<Media> Medias { get; set; }
        
        [JsonProperty("images")]
        public virtual ICollection<Image> Images { get; set; }
        
        [JsonProperty("translations")]
        public virtual ICollection<Translation> Translations { get; set; }

        public Season()
        {
        }

        public Season(SeasonAppends? s, int tvId)
        {
            Id = s.Id;
            Title = s.Name;
            AirDate = s.AirDate;
            EpisodeCount = s.Episodes.Length;
            Overview = s.Overview;
            Poster = s.PosterPath;
            SeasonNumber = s.SeasonNumber;
            TvId = tvId;
        }
    }
}