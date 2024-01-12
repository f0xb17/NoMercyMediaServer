using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieKeywords : SharedKeywords
{
    [JsonProperty("keywords")] public override List<Keyword>? Results { get; set; } = new();
}