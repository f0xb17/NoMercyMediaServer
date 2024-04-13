#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Newtonsoft.Json;

namespace NoMercy.Providers.MusicBrainz.Models;

public class Tag
{
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
}

public class Media
{
    [JsonProperty("track-count")] public int TrackCount { get; set; }
    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("format")] public string Format { get; set; }
    [JsonProperty("format-id")] public Guid? FormatId { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("tracks")] public Track[] Tracks { get; set; }
}

public class MediaDetails: Media
{
    [JsonProperty("discs")] public Disc[] Discs { get; set; }
    [JsonProperty("track-offset")] public int TrackOffset { get; set; }
}

public class Work
{
    [JsonProperty("attributes")] public object[] Attributes { get; set; }
    [JsonProperty("disambiguation")] public string Disambiguation { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("iswcs")] public string[] Iswcs { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("languages")] public string[] Languages { get; set; }
    [JsonProperty("relations")] public WorkRelation[] Relations { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("type-id")] public Guid? TypeId { get; set; }
}

public class TextRepresentation
{
    [JsonProperty("script")] public string Script { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
}

