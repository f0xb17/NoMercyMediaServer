using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.People;
using PersonGender = NoMercy.Providers.TMDB.Models.People.Gender;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Person : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("adult")] public bool Adult { get; set; } = false;
        [JsonProperty("also_known_as")] public string? AlsoKnownAs { get; set; }
        [JsonProperty("biography")] public string? Biography { get; set; }
        [JsonProperty("birthday")] public DateTime? BirthDay { get; set; }
        [JsonProperty("deathday")] public DateTime? DeathDay { get; set; } = null;

        [Column("Gender")]
        [JsonProperty("gender")]
        [System.Text.Json.Serialization.JsonIgnore]
        public PersonGender _gender { get; set; }

        [JsonProperty("homepage")] public string? Homepage { get; set; }
        [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
        [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
        [JsonProperty("name")] public string Name { get; set; } = String.Empty;
        [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
        [JsonProperty("popularity")] public float? Popularity { get; set; }
        [JsonProperty("profile_path")] public string? Profile { get; set; }
        [JsonProperty("title_sort")] public string TitleSort { get; set; } = String.Empty;

        [JsonProperty("casts")] public virtual ICollection<Cast>? Casts { get; set; }
        [JsonProperty("crews")] public virtual ICollection<Crew>? Crews { get; set; }

        [NotMapped]
        public string Gender
        {
            get => _gender.ToString();
            set => _gender = Enum.Parse<PersonGender>(value);
        }

        public Person()
        {
        }

        public Person(PersonAppends? person)
        {
            Id = person.Id;
            Adult = person.Adult;
            AlsoKnownAs = person.AlsoKnownAs?.Length > 0 ? JsonConvert.SerializeObject(person.AlsoKnownAs) : null;
            Biography = person.Biography;
            BirthDay = person.BirthDay;
            DeathDay = person.DeathDay;
            _gender = person.Gender;
            Homepage = person.Homepage?.ToString();
            ImdbId = person.ImdbId;
            KnownForDepartment = person.KnownForDepartment;
            Name = person.Name;
            PlaceOfBirth = person.PlaceOfBirth;
            Popularity = person.Popularity;
            Profile = person.ProfilePath;
            TitleSort = person.Name;
        }
    }
}