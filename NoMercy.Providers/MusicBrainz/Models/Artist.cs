#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Providers.MusicBrainz.Models;

public class Artist
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    
    [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
    public Tag[] Tags { get; set; }
    
    [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
    public GenreDetails[] Genres { get; set; }
    
    [JsonProperty("iso-3166-1-codes", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Iso31661Codes { get; set; }
    
    [JsonProperty("iso-3166-2-codes", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Iso31662Codes { get; set; }
}

public class ArtistDetails: Artist
{
    [JsonProperty("isnis")] public string[] Isnis { get; set; }
    [JsonProperty("end_area")] public object ArtistAppendsEndArea { get; set; }
    [JsonProperty("gender-id")] public Guid GenderId { get; set; }
    [JsonProperty("area")] public Area Area { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("begin_area")] public Area ArtistAppendsBeginArea { get; set; }
    [JsonProperty("end-area")] public object EndArea { get; set; }
    [JsonProperty("life-span")] public LifeSpan LifeSpan { get; set; }
    [JsonProperty("begin-area")] public Area BeginArea { get; set; }
    [JsonProperty("ipis")] public string[] Ipis { get; set; }
}

public class ArtistAppends: Artist
{
    [JsonProperty("works")] public Work[] Works { get; set; }
    [JsonProperty("releases")] public Release[] Releases { get; set; }
    [JsonProperty("release-groups")] public ReleaseGroup[] ReleaseGroups { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("recordings")] public Recording[] Recordings { get; set; }
}

public class Area
{
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("type")] public object Type { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    
    [JsonProperty("iso-3166-1-codes", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Iso31661Codes { get; set; }
    
    [JsonProperty("iso-3166-2-codes", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Iso31662Codes { get; set; }
}

public class LifeSpan
{
    
    [JsonProperty("begin")]
    [System.Text.Json.Serialization.JsonIgnore]
    
    private string _begin { get; set; }
    
    [NotMapped]
    public DateTime? Begin
    {
        get => DateTimeParser.ParseDateTime(_begin);
        set => _begin = value?.ToString() ?? "";
    }

    [JsonProperty("end")]
    [System.Text.Json.Serialization.JsonIgnore]
    
    private string _end { get; set; }
    
    [NotMapped]
    public DateTime? End
    {
        get => DateTimeParser.ParseDateTime(_end);
        set => _end = value?.ToString() ?? "";
    }
    
    [JsonProperty("ended")] public bool Ended { get; set; }
}
