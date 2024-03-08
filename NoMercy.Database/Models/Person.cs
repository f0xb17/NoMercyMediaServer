using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.People;
using PersonGender = NoMercy.Providers.TMDB.Models.People.Gender;
using PersonExternalIds = NoMercy.Providers.TMDB.Models.People.PersonExternalIds;

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
        [JsonProperty("deathday")] public DateTime? DeathDay { get; set; }

        [JsonProperty("homepage")] public string? Homepage { get; set; }
        [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
        [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
        [JsonProperty("name")] public string Name { get; set; } = String.Empty;
        [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
        [JsonProperty("popularity")] public double Popularity { get; set; }
        [JsonProperty("profile_path")] public string? Profile { get; set; }
        [JsonProperty("title_sort")] public string TitleSort { get; set; } = String.Empty;
        
        [JsonProperty("casts")] public virtual ICollection<Cast> Casts { get; set; }
        [JsonProperty("crews")] public virtual ICollection<Crew> Crews { get; set; }
        [JsonProperty("images")] public virtual ICollection<Image> Images { get; set; }
        [JsonProperty("translations")] public virtual ICollection<Translation> Translations { get; set; }

        [Column("Gender")]
        [JsonProperty("gender")]
        [System.Text.Json.Serialization.JsonIgnore]
        public PersonGender _gender { get; set; }

        [NotMapped]
        [JsonProperty("Gender")]
        public string Gender
        {
            get => _gender.ToString();
            set => _gender = Enum.Parse<PersonGender>(value);
        }
        
        [Column("ExternalIds")]
        [JsonProperty("external_ids")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string? _externalIds { get; set; }

        [NotMapped]
        public PersonExternalIds? ExternalIds
        {
            get => _externalIds is null ? null : JsonConvert.DeserializeObject<PersonExternalIds>(_externalIds);
            set => _externalIds = JsonConvert.SerializeObject(value);
        }

        public Person()
        {
        }

        public Person(PersonAppends person)
        {
            Id = person.Id;
            Adult = person.Adult;
            AlsoKnownAs = person.AlsoKnownAs?.Length > 0 ? JsonConvert.SerializeObject(person.AlsoKnownAs) : null;
            Biography = person.Biography;
            BirthDay = person.BirthDay;
            DeathDay = person.DeathDay;
            _gender = person.Gender;
            _externalIds = JsonConvert.SerializeObject(person.ExternalIds);
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