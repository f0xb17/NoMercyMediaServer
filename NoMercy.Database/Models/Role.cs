#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(CreditId), IsUnique = true)]
    [Index(nameof(GuestStarId), IsUnique = true)]
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("character")] public string? Character { get; set; }
        [JsonProperty("episode_count")] public int EpisodeCount { get; set; }
        [JsonProperty("order")] public int? Order { get; set; }

        [JsonProperty("credit_id")] public string? CreditId { get; set; }
        public virtual Cast? Cast { get; set; }

        [JsonProperty("guest_star_id")] public int? GuestStarId { get; set; }
        public virtual GuestStar? GuestStar { get; set; }

        public Role()
        {
        }

        public Role(NoMercy.Providers.TMDB.Models.Shared.AggregatedCreditRole role)
        {
            Character = role.Character;
            EpisodeCount = role.EpisodeCount;
            Order = role.Order;
            CreditId = role.CreditId;
        }

        public Role(NoMercy.Providers.TMDB.Models.Shared.Cast cast)
        {
            Character = cast.Character;
            CreditId = cast.CreditId;
            Order = cast.Order;
            EpisodeCount = 0;
        }

        public Role(NoMercy.Providers.TMDB.Models.Shared.GuestStar guest)
        {
            Character = guest.CharacterName;
            CreditId = guest.CreditId;
            Order = guest.Order;
            EpisodeCount = 0;
        }
    }
}