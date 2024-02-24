using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.Movies;

public class Movie
{
    [JsonProperty("adult")] public bool Adult { get; set; }

    [JsonProperty("backdrop_path")] public string? BackdropPath { get; set; }

    [JsonProperty("genres")] public int[]? GenresIds { get; set; } = [];

    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }

    [JsonProperty("original_title")] public string OriginalTitle { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("popularity")] public double Popularity { get; set; }

    [JsonProperty("poster_path")] public string? PosterPath { get; set; }

    [JsonProperty("tagline")] public string? Tagline { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }
    
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }

    [JsonProperty("video")] public string? Video { get; set; }

    [JsonProperty("vote_average")] public double VoteAverage { get; set; }

    [JsonProperty("vote_count")] public int VoteCount { get; set; }
}