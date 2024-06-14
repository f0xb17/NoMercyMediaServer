#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(CreditId), nameof(EpisodeId), IsUnique = true)]
public class GuestStar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("credit_id")] public string? CreditId { get; set; }

    [JsonProperty("episode_id")] public int EpisodeId { get; set; }
    public Episode Episode { get; set; }

    [JsonProperty("person_id")] public int PersonId { get; set; }
    public Person Person { get; set; }

    public GuestStar()
    {
    }

    public GuestStar(TmdbGuestStar cast, TmdbEpisode? episodeAppends)
    {
        CreditId = cast.CreditId;
        PersonId = cast.Id;
        if (episodeAppends != null) EpisodeId = episodeAppends.Id;
    }
}