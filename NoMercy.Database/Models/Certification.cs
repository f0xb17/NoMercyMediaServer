using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.Certifications;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Iso31661), nameof(Rating), IsUnique = true)]
    public class Certification
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Iso31661 { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public int Order { get; set; }
        
        public Certification()
        {
            
        }

        public Certification(string country, TvShowCertification certification)
        {
            Iso31661 = country;
            Rating = certification.Rating;
            Meaning = certification.Meaning;
            Order = certification.Order;
        }
        
        public Certification(string country, MovieCertification certification)
        {
            Iso31661 = country;
            Rating = certification.Rating;
            Meaning = certification.Meaning;
            Order = certification.Order;
        }

    }
}