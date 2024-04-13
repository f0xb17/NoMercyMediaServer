#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;
using TMDBCrew = NoMercy.Providers.TMDB.Models.Shared.Crew;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(CreditId), IsUnique = true)]
public class Job
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("task")] public string? Task { get; set; }
    [JsonProperty("episode_count")] public int? EpisodeCount { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }
    
    [JsonProperty("credit_id")] public string? CreditId { get; set; }
    public virtual Crew? Crew { get; set; }

    public Job()
    {
    }

    public Job(AggregatedCrewJob job)
    {
        Task = job.Job ?? "Unknown";
        EpisodeCount = job.EpisodeCount;
        CreditId = job.CreditId;
        Order = job.Order;
    }

    public Job(TMDBCrew crew)
    {
        Task = crew.Job ?? "Unknown";
        CreditId = crew.CreditId;
    }
}