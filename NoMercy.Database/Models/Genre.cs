using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Genre
    {
        public Genre(Providers.TMDB.Models.Shared.Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
        }
        
        public Genre()
        {
            
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        
    }
}