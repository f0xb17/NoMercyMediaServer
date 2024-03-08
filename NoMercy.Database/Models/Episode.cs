using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Episode;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Episode : ColorPalettes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")] public string? Title { get; set; }

        [JsonProperty("air_date")] public DateTime? AirDate { get; set; }

        [JsonProperty("episode_number")] public int EpisodeNumber { get; set; }

        [JsonProperty("imdb_id")] public string? ImdbId { get; set; }

        [JsonProperty("overview")] public string? Overview { get; set; }

        [JsonProperty("production_code")] public string? ProductionCode { get; set; }

        [JsonProperty("season_number")] public int SeasonNumber { get; set; }

        [JsonProperty("still")] public string? Still { get; set; }

        [JsonProperty("tvdb_id")] public int? TvdbId { get; set; }

        [JsonProperty("vote_average")] public float? VoteAverage { get; set; }

        [JsonProperty("vote_count")] public int? VoteCount { get; set; }


        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }
        
        [JsonProperty("season_id")] public int SeasonId { get; set; }
        public virtual Season Season { get; set; }
        
        [JsonProperty("casts")] 
        public virtual ICollection<Cast> Casts { get; set; }

        [JsonProperty("crews")] 
        public virtual ICollection<Crew> Crews { get; set; }

        [JsonProperty("special_items")] 
        public virtual ICollection<SpecialItem> SpecialItems { get; set; }

        [JsonProperty("video_files")] 
        public virtual ICollection<VideoFile> VideoFiles { get; set; } = new HashSet<VideoFile>();

        [JsonProperty("medias")] 
        public virtual ICollection<Media> Media { get; set; }

        [JsonProperty("images")] 
        public virtual ICollection<Image> Images { get; set; }

        [JsonProperty("guest_stars")] 
        public virtual ICollection<GuestStar> GuestStars { get; set; }

        [JsonProperty("files")] 
        public virtual ICollection<File> Files { get; set; }

        [JsonProperty("translations")] 
        public virtual ICollection<Translation> Translations { get; set; }

        public Episode()
        {
        }

        public Episode(EpisodeAppends? e, int tvId, int seasonId)
        {
            Id = e.Id;
            Title = e.Name;
            AirDate = e.AirDate;
            EpisodeNumber = e.EpisodeNumber;
            ImdbId = e.ExternalIds?.ImdbId;
            Overview = e.Overview;
            ProductionCode = e.ProductionCode;
            SeasonNumber = e.SeasonNumber;
            Still = e.StillPath;
            TvdbId = e.ExternalIds?.TvdbId;
            VoteAverage = e.VoteAverage;
            VoteCount = e.VoteCount;

            TvId = tvId;
            SeasonId = seasonId;
        }
    }
}