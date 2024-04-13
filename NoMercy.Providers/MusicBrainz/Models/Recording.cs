#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Providers.MusicBrainz.Models;

public class Recording
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("genres")] public object[] Genres { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}

public class RecordingAppends : Recording
{
    [JsonProperty("first-release-date")]
    [System.Text.Json.Serialization.JsonIgnore]
    
    private string _firstReleaseDate { get; set; }
    
    [NotMapped]
    public DateTime? FirstReleaseDate => DateTimeParser.ParseDateTime(_firstReleaseDate);

    [JsonProperty("media")] public Media[] Media { get; set; }
    [JsonProperty("tags")] public Tag[] Tags { get; set; }
    [JsonProperty("releases")] public Release[] Releases { get; set; }
    [JsonProperty("artist-credit")] public ArtistCredit[] ArtistCredit { get; set; }
}

public class ArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("artist")] public Artist Artist { get; set; }
}