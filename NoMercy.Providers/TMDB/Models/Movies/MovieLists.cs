using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieLists : PaginatedResponse<Movie>
{
    [JsonProperty("id")] public int Id { get; set; }

}