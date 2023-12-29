﻿using Newtonsoft.Json;
using NoMercy.TMDBApi.Models.Shared;

namespace NoMercy.TMDBApi.Models.Season;

public class Credits
{
    [JsonProperty("cast")] public Cast[] Cast { get; set; } = Array.Empty<Cast>();

    [JsonProperty("crew")] public Crew[] Crew { get; set; } = Array.Empty<Crew>();
}