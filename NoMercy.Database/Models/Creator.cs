#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(PersonId), nameof(TvId))]
    public class Creator
    {
        [JsonProperty("person_id")] public required int PersonId { get; set; }
        public virtual Person Person { get; set; }

        [JsonProperty("tv_id")] public required int TvId { get; set; }
        public virtual Tv Tv { get; set; }
    }
}