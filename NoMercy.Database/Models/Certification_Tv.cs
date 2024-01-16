using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{    
    [PrimaryKey(nameof(CertificationId), nameof(TvId))]
    public class Certification_Tv
    {
        public required int CertificationId { get; set; }
        public required int TvId { get; set; }

        public virtual Certification Certification { get; } = null!;
        public virtual Tv Tv { get; set; }        
    }
}