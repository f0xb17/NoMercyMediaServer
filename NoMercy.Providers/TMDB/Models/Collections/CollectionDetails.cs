using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Collections;

public class CollectionDetails: Collection
{
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("parts")] public Movies.Movie[] Parts { get; set; }
}