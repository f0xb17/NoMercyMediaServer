using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(GenreId), nameof(TrackId))]
    public class MusicGenreTrack
    {
        [JsonProperty("genre_id")] public Guid GenreId { get; set; }
        public virtual MusicGenre Genre { get; set; }

        [JsonProperty("track_id")] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        public MusicGenreTrack()
        {
        }

        public MusicGenreTrack(Guid genreId, Guid trackId)
        {
            GenreId = genreId;
            TrackId = trackId;
        }
    }
}