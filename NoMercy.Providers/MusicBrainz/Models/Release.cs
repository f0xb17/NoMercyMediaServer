#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Providers.MusicBrainz.Models;

public class Release
{
    [JsonProperty("barcode")] public string Barcode { get; set; }
    [JsonProperty("country")] public string Country { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    // [JsonProperty("genres")] public object[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("media")] public Media[] Media { get; set; }
    [JsonProperty("packaging")] public string Packaging { get; set; }
    [JsonProperty("packaging-id")] public Guid? PackagingId { get; set; }
    [JsonProperty("quality")] public string Quality { get; set; }
    [JsonProperty("release-events")] public ReleaseEvent[] ReleaseEvents { get; set; }
    [JsonProperty("release-group")] public ReleaseGroup ReleaseGroup { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
    [JsonProperty("status-id")] public Guid? StatusId { get; set; }
    [JsonProperty("text-representation")] public TextRepresentation TextRepresentation { get; set; }
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("date")]
    [System.Text.Json.Serialization.JsonIgnore]
    
    private string _date { get; set; }
    
    [NotMapped]
    public DateTime? Date => DateTimeParser.ParseDateTime(_date);
}

public class ReleaseAppends : Release
{
    // [JsonProperty("aliases")] public object[] Aliases { get; set; }
    // [JsonProperty("annotation")] public object Annotation { get; set; }
    [JsonProperty("artist-credit")] public ReleaseArtistCredit[] ArtistCredit { get; set; }
    // [JsonProperty("asin")] public object Asin { get; set; }
    [JsonProperty("collections")] public Collection[] Collections { get; set; }
    [JsonProperty("cover-art-archive")] public CoverArtArchive CoverArtArchive { get; set; }
    [JsonProperty("label-info")] public LabelInfo[] LabelInfo { get; set; }
    [JsonProperty("relations")] public WorkRelation[] Relations { get; set; }
    [JsonProperty("tags")] public Tag[] Tags { get; set; }
}

public class ReleaseEvent
{
    [JsonProperty("area")] public Area Area { get; set; }
    
    // [JsonProperty("date")]
    // public CustomDateTime? Date { get; set; }
    
    [JsonProperty("date")]
    [System.Text.Json.Serialization.JsonIgnore]
    
    private string _date { get; set; }
    
    [NotMapped]
    public DateTime? Date
    {
        get => DateTimeParser.ParseDateTime(_date);
        set => _date = value?.ToString() ?? "";
    }
}

public class ReleaseArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
    [JsonProperty("artist")] public Artist Artist { get; set; }
}

public class Alias: LifeSpan
{
    [JsonProperty("locale")] public string? Locale { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("primary")] public bool? Primary { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class Collection
{
    [JsonProperty("editor")] public string Editor { get; set; }
    [JsonProperty("entity-type")] public string EntityType { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("release-count")] public int ReleaseCount { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class CoverArtArchive
{
    [JsonProperty("artwork")] public bool Artwork { get; set; }
    [JsonProperty("back")] public bool Back { get; set; }
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("darkened")] public bool Darkened { get; set; }
    [JsonProperty("front")] public bool Front { get; set; }
}

public class LabelInfo
{
    [JsonProperty("catalog-number")] public string CatalogNumber { get; set; }
    [JsonProperty("label")] public Label Label { get; set; }
}

public class Label
{
    [JsonProperty("aliases")] public Alias[] Aliases { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("genres")] public GenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("label-code")] public string? LabelCode { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("tags")] public Tag[] Tags { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class Disc
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("offset-count")] public int OffsetCount { get; set; }
    [JsonProperty("offsets")] public int[] Offsets { get; set; }
    [JsonProperty("sectors")] public int Sectors { get; set; }
}

public class Track
{
    [JsonProperty("artist-credit")] public ReleaseArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("number")] public int Number { get; set; }
    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("recording")] public TrackRecording Recording { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}

public class TrackRecording
{
    [JsonProperty("aliases")] public Alias[] Aliases { get; set; }
    [JsonProperty("artist-credit")] public RecordingArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("first-release-date")] public string FirstReleaseDate { get; set; }
    [JsonProperty("genres")] public GenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("isrcs")] public string[] Isrcs { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("relations")] public RecordingRelation[] Relations { get; set; }
    [JsonProperty("tags")] public Tag[] Tags { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
}

public class RecordingArtistCredit
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("artist")] public PurpleArtist Artist { get; set; }
    [JsonProperty("joinphrase")] public string Joinphrase { get; set; }
}

public class PurpleArtist
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    // [JsonProperty("label-code")] public object LabelCode { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("sort-name")] public string SortName { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }

    [JsonProperty("iso-3166-1-codes", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Iso31661Codes { get; set; }
}

public class RecordingRelation: LifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public AttributeValues AttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }

    [JsonProperty("artist", NullValueHandling = NullValueHandling.Ignore)]
    public PurpleArtist Artist { get; set; }

    [JsonProperty("attribute-credits", NullValueHandling = NullValueHandling.Ignore)]
    public AttributeCredits AttributeCredits { get; set; }

    [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
    public PurpleArtist Label { get; set; }

    [JsonProperty("work", NullValueHandling = NullValueHandling.Ignore)]
    public Work Work { get; set; }

    [JsonProperty("recording", NullValueHandling = NullValueHandling.Ignore)]
    public RelationRecording Recording { get; set; }
}

public class AttributeCredits
{
    [JsonProperty("Rhodes piano", NullValueHandling = NullValueHandling.Ignore)]
    public string RhodesPiano { get; set; }

    [JsonProperty("synthesizer", NullValueHandling = NullValueHandling.Ignore)]
    public string Synthesizer { get; set; }

    [JsonProperty("drums (drum set)", NullValueHandling = NullValueHandling.Ignore)]
    public string? DrumsDrumSet { get; set; }

    [JsonProperty("handclaps", NullValueHandling = NullValueHandling.Ignore)]
    public string Handclaps { get; set; }

    [JsonProperty("Hammond organ", NullValueHandling = NullValueHandling.Ignore)]
    public string HammondOrgan { get; set; }

    [JsonProperty("keyboard", NullValueHandling = NullValueHandling.Ignore)]
    public string Keyboard { get; set; }

    [JsonProperty("drum machine", NullValueHandling = NullValueHandling.Ignore)]
    public string DrumMachine { get; set; }

    [JsonProperty("foot stomps", NullValueHandling = NullValueHandling.Ignore)]
    public string FootStomps { get; set; }

    [JsonProperty("Wurlitzer electric piano", NullValueHandling = NullValueHandling.Ignore)]
    public string WurlitzerElectricPiano { get; set; }
}

public class AttributeValues
{
    [JsonProperty("task", NullValueHandling = NullValueHandling.Ignore)]
    public string Task { get; set; }
}

public class RelationRecording
{
    [JsonProperty("artist-credit")] public RecordingArtistCredit[] ArtistCredit { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    // [JsonProperty("isrcs")] public object[] Isrcs { get; set; }
    [JsonProperty("length")] public int? Length { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("video")] public bool Video { get; set; }
}

public class WorkRelation: LifeSpan
{
    [JsonProperty("attribute-ids")] public Dictionary<string, Guid> AttributeIds { get; set; }
    [JsonProperty("attribute-values")] public AttributeValues AttributeValues { get; set; }
    [JsonProperty("attributes")] public string[] Attributes { get; set; }
    [JsonProperty("direction")] public string Direction { get; set; }
    [JsonProperty("source-credit")] public string SourceCredit { get; set; }
    [JsonProperty("target-credit")] public string TargetCredit { get; set; }
    [JsonProperty("target-type")] public string TargetType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }

    [JsonProperty("artist", NullValueHandling = NullValueHandling.Ignore)]
    public Artist Artist { get; set; }

    [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
    public Label Label { get; set; }

    [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    public Url Url { get; set; }
}

public class Url
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("resource")] public Uri Resource { get; set; }
}

public class ReleaseGroup
{
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("first-release-date")] public DateTime? FirstReleaseDate { get; set; }
    [JsonProperty("genres")] public GenreDetails[] Genres { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("primary-type")] public string PrimaryType { get; set; }
    [JsonProperty("primary-type-id")] public Guid? PrimaryTypeId { get; set; }
    [JsonProperty("releases")] public Release[] Releases { get; set; }
    [JsonProperty("secondary-type-ids")] public Guid[] SecondaryTypeIds { get; set; }
    [JsonProperty("secondary-types")] public string[] SecondaryTypes { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
}