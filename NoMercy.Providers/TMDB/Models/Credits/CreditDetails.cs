using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Credits;

public class CreditDetails
{
    [JsonProperty("credit_type")] public string CreditType { get; set; } = string.Empty;

    [JsonProperty("department")] public string Department { get; set; } = string.Empty;

    [JsonProperty("job")] public string Job { get; set; } = string.Empty;

    [JsonProperty("media")] public Media? Media { get; set; }

    [JsonProperty("media_type")] public string? MediaType { get; set; }

    [JsonProperty("id")] public string Id { get; set; } = string.Empty;

    [JsonProperty("person")] public Person Person { get; set; } = new();
}

public class Media
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; } = string.Empty;

    [JsonProperty("original_name")] public string OriginalName { get; set; } = string.Empty;

    [JsonProperty("character")] public string Character { get; set; } = string.Empty;

    [JsonProperty("episodes")] public Episode.Episode[] Episodes { get; set; } = [];

    [JsonProperty("seasons")] public Season.Season[] Seasons { get; set; } = [];
}

public class S : Season.Season
{
    [JsonProperty("air_date")] public new DateTime AirDate { get; set; }

    [JsonProperty("poster_path")] public string? PosterPath { get; set; }

    [JsonProperty("season_number")] public int SeasonNumber { get; set; } = 0;
}

public class Person
{
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("id")] public int Id { get; set; }
}