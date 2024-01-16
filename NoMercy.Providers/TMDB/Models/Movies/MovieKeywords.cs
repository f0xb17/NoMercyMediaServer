using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieKeywords : SharedKeywords
{
    [JsonProperty("keywords")] public override Keyword[] Results { get; set; } = [];
}