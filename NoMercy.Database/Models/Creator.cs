#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(PersonId), nameof(TvId))]
public class Creator
{
    [JsonProperty("person_id")] public int PersonId { get; set; }
    public Person Person { get; set; }

    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    public Creator(TmdbCreatedBy input, TmdbTvShowAppends show)
    {
        PersonId = input.Id;
        TvId = show.Id;
    }

    public Creator()
    {
        //
    }
}