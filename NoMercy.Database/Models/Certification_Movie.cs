using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CertificationId), nameof(MovieId))]
    public class Certification_Movie
    {
        public required int CertificationId { get; set; }
        public required int MovieId { get; set; }

        public virtual Certification Certification { get; } = null!;
        public virtual Movie Movie { get; } = null!;        
    }
}