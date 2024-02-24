using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        public virtual ICollection<GenreMovie> GenreMovies { get; set; }
        public virtual ICollection<GenreTv> GenreTvShows { get; set; }

        public Genre()
        {
        }

        public Genre(Providers.TMDB.Models.Shared.Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
        }
    }
}