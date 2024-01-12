using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{    
    [PrimaryKey(nameof(Priority), nameof(Country), nameof(ProviderId))]
    public class Priority_Provider
    {
        public required int Priority { get; set; }
        public required string Country { get; set; }
        public required string ProviderId { get; set; }

        public virtual Provider Provider { get; set; }        
    }
}