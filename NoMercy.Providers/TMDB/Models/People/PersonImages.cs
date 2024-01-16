using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonImages
{
    [JsonProperty("profiles")] public Profile[] Profiles { get; set; } = [];
}