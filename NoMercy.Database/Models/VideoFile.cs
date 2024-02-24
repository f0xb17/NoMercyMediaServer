using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Filename), IsUnique = true)]
    public class VideoFile : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        [JsonProperty("duration")] public string? Duration { get; set; }
        [JsonProperty("filename")] public string? Filename { get; set; }
        [JsonProperty("folder")] public string? Folder { get; set; }
        [JsonProperty("host_folder")] public string? HostFolder { get; set; }
        [JsonProperty("languages")] public string? Languages { get; set; }
        [JsonProperty("quality")] public string? Quality { get; set; }
        [JsonProperty("share")] public string? Share { get; set; }
        [JsonProperty("subtitles")] public string? Subtitles { get; set; }
        [JsonProperty("chapters")] public string? Chapters { get; set; }

        [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }

        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}