using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TMDBGuestStar = NoMercy.Providers.TMDB.Models.Shared.GuestStar;
using TMDBEpisode = NoMercy.Providers.TMDB.Models.Episode.Episode;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(CreditId), nameof(EpisodeId), IsUnique = true)]
    public class GuestStar
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("credit_id")] public string CreditId { get; set; }
        
        [JsonProperty("episode_id")] public int EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }
        
        [JsonProperty("person_id")] public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        public GuestStar()
        {
        }

        public GuestStar(TMDBGuestStar cast, TMDBEpisode? episodeAppends)
        {
            CreditId = cast.CreditId;
            PersonId = cast.Id;
            EpisodeId = episodeAppends.Id;
        }
    }
}