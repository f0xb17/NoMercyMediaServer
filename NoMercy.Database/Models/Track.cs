#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;using NoMercy.Database;
using NoMercy.Providers.MusixMatch.Models;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Folder))]
    [Index(nameof(Filename), nameof(HostFolder), IsUnique = true)]
    public class Track: ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("track")] public int TrackNumber { get; set; }
        [JsonProperty("disc")] public int DiscNumber { get; set; }
        [JsonProperty("cover")] public string? Cover { get; set; }
        [JsonProperty("date")] public DateTime? Date { get; set; }
        [JsonProperty("filename")] public string? Filename { get; set; }
        [JsonProperty("duration")] public string? Duration { get; set; }
        [JsonProperty("quality")] public int? Quality { get; set; }
        
        [Column("Lyrics")]
        [System.Text.Json.Serialization.JsonIgnore]
        
        public string? _lyrics { get; set; }

        [NotMapped]
        [JsonProperty("lyrics")]
        public Lyric[]? Lyrics
        {
            get => _lyrics is null ? null : JsonConvert.DeserializeObject<Lyric[]>(_lyrics);
            set => _lyrics = JsonConvert.SerializeObject(value);
        }
        
        [JsonProperty("folder")] public string? Folder { get; set; }
        [JsonProperty("host_folder")] public string? HostFolder { get; set; }
        
        [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
        public virtual Folder LibraryFolder { get; set; }
        
        [JsonProperty("album_track")]
        public virtual ICollection<AlbumTrack> AlbumTrack { get; set; }
        [JsonProperty("artist_track")]
        public virtual ICollection<ArtistTrack> ArtistTrack { get; set; }
        [JsonProperty("library_track")]
        public virtual ICollection<LibraryTrack> LibraryTrack { get; set; }
        [JsonProperty("playlist_track")]
        public virtual ICollection<PlaylistTrack> PlaylistTrack { get; set; }
        [JsonProperty("images")]
        public virtual ICollection<Image> Images { get; set; }
        [JsonProperty("track_user")]
        public virtual ICollection<TrackUser> TrackUser { get; set; }
        [JsonProperty("music_genre_track")]
        public virtual ICollection<MusicGenreTrack> MusicGenreTrack { get; set; }
        [JsonProperty("music_plays")]
        public virtual ICollection<MusicPlay> MusicPlays { get; set; }
    }
    
}

public class Lyric
{
    [JsonProperty("text")] public string Text;
    [JsonProperty("time")] public LineTime Time;

    public class LineTime
    {
        [JsonProperty("total")] public double Total;
        [JsonProperty("minutes")] public int Minutes;
        [JsonProperty("seconds")] public int Seconds;
        [JsonProperty("hundredths")] public int Hundredths;
    }
}
