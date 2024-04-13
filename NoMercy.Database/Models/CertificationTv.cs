#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(CertificationId), nameof(TvId))]
    public class CertificationTv
    {
        [JsonProperty("certification_id")] public int CertificationId { get; set; }
        public virtual Certification Certification { get; set; }

        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }

        public CertificationTv()
        {
        }

        public CertificationTv(Certification? certification, TvShowAppends? show)
        {
            if (certification == null || show == null) return;
            CertificationId = certification.Id;
            TvId = show.Id;
        }
    }
}