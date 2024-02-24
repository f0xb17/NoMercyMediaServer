using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(AlbumId), nameof(MusicGenreId))]
    public class AlbumMusicGenre
    {
        [JsonProperty("album_id")] public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
        
        [JsonProperty("music_genre_id")] public Guid MusicGenreId { get; set; }
        public virtual MusicGenre MusicGenre { get; set; }

        public AlbumMusicGenre()
        {
        }

        public AlbumMusicGenre(Guid albumId, Guid musicGenreId)
        {
            AlbumId = albumId;
            MusicGenreId = musicGenreId;
        }
    }
}