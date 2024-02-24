using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(MovieId), nameof(VideoFileId), nameof(UserId), IsUnique = true)]
    public class UserData : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        [JsonProperty("name")] public int? Rating { get; set; }
        [JsonProperty("played")] public bool? Played { get; set; }
        [JsonProperty("play_count")] public int? PlayCount { get; set; }
        [JsonProperty("is_favorite")] public bool? IsFavorite { get; set; }

        [JsonProperty("playback_position_ticks")]
        public int? PlaybackPositionTicks { get; set; }

        [JsonProperty("last_played_date")] public string? LastPlayedDate { get; set; }
        [JsonProperty("audio")] public string? Audio { get; set; }
        [JsonProperty("subtitle")] public string? Subtitle { get; set; }
        [JsonProperty("subtitle_type")] public string? SubtitleType { get; set; }
        [JsonProperty("time")] public int? Time { get; set; }
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("user_id")] public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        [JsonProperty("episode_id")] public int? TvId { get; set; }
        public virtual Tv? Tv { get; set; }

        [JsonProperty("special_id")] public string? SpecialId { get; set; }
        public virtual Special? Special { get; set; }

        [JsonProperty("video_file_id")] public Ulid? VideoFileId { get; set; }
        public virtual VideoFile VideoFile { get; set; }
    }
}