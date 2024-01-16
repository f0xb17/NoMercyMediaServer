using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.People;
using PersonGender = NoMercy.Providers.TMDB.Models.People.Gender;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Person: ColorPaletteTimeStamps
    {
        public Person(PersonAppends person)
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
        public Person()
        { }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public bool Adult { get; set; } = false;
        [StringLength(255)]
        public string? AlsoKnownAs { get; set; }
        [StringLength(255)]
        public string? Biography { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? DeathDay { get; set; } = null;
        
        [Column("Gender")]
        public PersonGender _gender { get; set; }
        [StringLength(255)]
        public string? Homepage { get; set; }
        [StringLength(7)]
        public string? ImdbId { get; set; }
        [StringLength(255)]
        public string? KnownForDepartment { get; set; }
        [StringLength(255)]
        public string Name { get; set; } = String.Empty;
        [StringLength(255)]
        public string? PlaceOfBirth { get; set; }
        public float? Popularity { get; set; }
        [StringLength(255)]
        public string? Profile { get; set; }
        [StringLength(255)]
        public string TitleSort { get; set; } = String.Empty;
        
        public virtual Cast? Casts { get; } = new();
        public virtual Crew? Crews { get; } = new();

        [NotMapped]
        public string Gender
        {
            get => _gender.ToString();
            set => _gender = Enum.Parse<PersonGender>(value);
        }
    }
    
}