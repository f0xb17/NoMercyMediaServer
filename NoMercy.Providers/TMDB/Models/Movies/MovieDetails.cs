using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieDetails : Movie
{
    [JsonProperty("belongs_to_collection")]
    public Collection? BelongsToCollection { get; set; }

    [JsonProperty("budget")] public int Budget { get; set; }

    [JsonProperty("genres")] public List<Genre> Genres { get; set; } = new();

    [JsonProperty("homepage")] public Uri? Homepage { get; set; }

    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }

    [JsonProperty("production_companies")] public List<ProductionCompany> ProductionCompanies { get; set; } = new();

    [JsonProperty("production_countries")] public List<ProductionCountry> ProductionCountries { get; set; } = new();

    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }

    [JsonProperty("revenue")] public long Revenue { get; set; }

    [JsonProperty("runtime")] public int Runtime { get; set; }

    [JsonProperty("spoken_languages")] public List<SpokenLanguage> SpokenLanguages { get; set; } = new();

    [JsonProperty("status")] public string? Status { get; set; }

}