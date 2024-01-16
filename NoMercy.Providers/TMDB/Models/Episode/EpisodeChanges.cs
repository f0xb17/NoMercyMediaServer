﻿using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Episode;

public class EpisodeChanges
{
    [JsonProperty("changes")] public Change[] Changes { get; set; } = [];
}

public class Change
{

    [JsonProperty("key")] public string Key { get; set; } = string.Empty;

    [JsonProperty("items")] public Item[] Items { get; set; } = [];
}

public class Item
{
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;

    [JsonProperty("action")] public string Action { get; set; } = string.Empty;

    [JsonProperty("time")] public string Time { get; set; } = string.Empty;

    [JsonProperty("value")] public string Value { get; set; } = string.Empty;

    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; } = string.Empty;

    [JsonProperty("original_value")] public string OriginalValue { get; set; } = string.Empty;
}