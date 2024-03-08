using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Models.Collections;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Collection : ColorPalettes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("title_sort")] public string? TitleSort { get; set; }
        [JsonProperty("backdrop")] public string? Backdrop { get; set; }
        [JsonProperty("poster")] public string? Poster { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }

        [JsonProperty("parts")] public int Parts { get; set; }

        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }
        
        [JsonProperty("collection_movies")]
        public virtual ICollection<CollectionMovie> CollectionMovies { get; set; } = new HashSet<CollectionMovie>();
        
        [JsonProperty("translations")]
        public virtual ICollection<Translation> Translations { get; set; }
        
        [JsonProperty("images")]
        public virtual ICollection<Image> Images { get; set; }
        
        [JsonProperty("collection_user")] 
        public virtual ICollection<CollectionUser> CollectionUser { get; set; }

        public Collection()
        {
        }

        public Collection(CollectionAppends collection, Ulid libraryId)
        {
            Id = collection.Id;
            Title = collection.Name;
            TitleSort = collection.Name.TitleSort(collection.Parts.MinBy(movie => movie.ReleaseDate)?.ReleaseDate);
            Backdrop = collection.BackdropPath;
            Poster = collection.PosterPath;
            Overview = collection.Overview;
            Parts = collection.Parts.Length;
            LibraryId = libraryId;
        }
    }
}