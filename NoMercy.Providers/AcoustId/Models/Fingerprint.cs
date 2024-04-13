#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.AcoustId.Models;

public class Fingerprint
{
    [JsonProperty("results")] public FingerprintResult[] Results { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
}

public class FingerprintResult
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("recordings")] public FingerprintRecording[] Recordings { get; set; }
    [JsonProperty("score")] public double Score { get; set; }
}

public class FingerprintRecording
{
    [JsonProperty("artists")] public FingerprintArtist[] Artists { get; set; }
    [JsonProperty("duration")] public int Duration { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("releases")] public FingerprintReleaseGroups[]? ReleaseGroups { get; set; }
    [JsonProperty("sources")] public int Sources { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
}

public class FingerprintReleaseGroups
{
    [JsonProperty("artists")] public FingerprintArtist[] Artists { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("date")] public FingerprintDate Date { get; set; }
    [JsonProperty("id")] public Guid? Id { get; set; }
    [JsonProperty("medium_count")] public int? MediumCount { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("track_count")] public int? TrackCount { get; set; }
    
    [JsonProperty("mediums")] public FingerprintMedium[] Mediums { get; set; }
    [JsonProperty("releaseevents")] public FingerprintReleaseEvent[] Releaseevents { get; set; }
}

public class FingerprintMedium
{
    [JsonProperty("format")] public string? Format { get; set; }
    [JsonProperty("position")] public int? Position { get; set; }
    [JsonProperty("track_count")] public int? TrackCount { get; set; }
    [JsonProperty("tracks")] public FingerprintTrack[] Tracks { get; set; }
}

public class FingerprintTrack
{
    [JsonProperty("artists")] public FingerprintArtist[]? Artists { get; set; }
    [JsonProperty("id")] public Guid? Id { get; set; }
    [JsonProperty("position")] public int? Position { get; set; }
}

public class FingerprintArtist
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}

public class FingerprintReleaseEvent
{
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("date")] public FingerprintDate Date { get; set; }
}

public class FingerprintDate
{
    [JsonProperty("day")] public int Day { get; set; }
    [JsonProperty("month")] public int Month { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
}