using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CertificationId), nameof(MovieId))]
    public class Certification_Movie
    {
        public required string CertificationId { get; set; }
        public required int MovieId { get; set; }

        public virtual Certification Certification { get; set; }
        public virtual Movie Movie { get; set; }        
    }
}