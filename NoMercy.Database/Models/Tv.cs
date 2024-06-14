#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Models.TV;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Tv : ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
    [JsonProperty("titleSort")] public string? TitleSort { get; set; } = string.Empty;
    [JsonProperty("airDate")] public int? HaveEpisodes { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("duration")] public int? Duration { get; set; }
    [JsonProperty("first_air_date")] public DateTime? FirstAirDate { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("in_production")] public bool? InProduction { get; set; }
    [JsonProperty("last_episode_to_air")] public int? LastEpisodeToAir { get; set; }
    [JsonProperty("media_type")] public string? MediaType { get; set; }
    [JsonProperty("next_episode_to_air")] public int? NextEpisodeToAir { get; set; }
    [JsonProperty("number_of_items")] public int NumberOfEpisodes { get; set; }
    [JsonProperty("number_of_seasons")] public int? NumberOfSeasons { get; set; }
    [JsonProperty("origin_country")] public string? OriginCountry { get; set; }
    [JsonProperty("original_language")] public string? OriginalLanguage { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("popularity")] public double? Popularity { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("spoken_languages")] public string? SpokenLanguages { get; set; }
    [JsonProperty("status")] public string? Status { get; set; }
    [JsonProperty("tagline")] public string? Tagline { get; set; }
    [JsonProperty("trailer")] public string? Trailer { get; set; }
    [JsonProperty("tvdb_id")] public int? TvdbId { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("vote_average")] public double? VoteAverage { get; set; }
    [JsonProperty("vote_count")] public int? VoteCount { get; set; }

    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    public Library Library { get; set; }

    [JsonProperty("alternative_titles")] public ICollection<AlternativeTitle> AlternativeTitles { get; set; }
    [JsonProperty("casts")] public ICollection<Cast> Cast { get; set; }
    [JsonProperty("certifications")] public ICollection<CertificationTv> CertificationTvs { get; set; }
    [JsonProperty("crews")] public ICollection<Crew> Crew { get; set; }
    [JsonProperty("creators")] public ICollection<Creator> Creators { get; set; }
    [JsonProperty("genres")] public ICollection<GenreTv> GenreTvs { get; set; }
    [JsonProperty("keywords")] public ICollection<KeywordTv> KeywordTvs { get; set; }
    [JsonProperty("medias")] public ICollection<Media> Media { get; set; }
    [JsonProperty("images")] public ICollection<Image> Images { get; set; }
    [JsonProperty("seasons")] public ICollection<Season> Seasons { get; set; }
    [JsonProperty("translations")] public ICollection<Translation> Translations { get; set; }
    [JsonProperty("user_data")] public ICollection<UserData> UserData { get; set; }
    [JsonProperty("episodes")] public ICollection<Episode> Episodes { get; set; } = new HashSet<Episode>();
    [InverseProperty("TvFrom")] public ICollection<Recommendation> RecommendationFrom { get; set; }
    [InverseProperty("TvTo")] public ICollection<Recommendation> RecommendationTo { get; set; }
    [InverseProperty("TvFrom")] public ICollection<Similar> SimilarFrom { get; set; }
    [InverseProperty("TvTo")] public ICollection<Similar> SimilarTo { get; set; }
    [JsonProperty("tv_user")] public ICollection<TvUser> TvUser { get; set; }

    public Tv()
    {
    }

    public Tv(TmdbTvShowAppends tmdbTvShowAppends, Ulid libraryId, string folder, string mediaType)
    {
        Id = tmdbTvShowAppends.Id;
        Backdrop = tmdbTvShowAppends.BackdropPath;
        Duration = tmdbTvShowAppends.EpisodeRunTime?.Length > 0
            ? (int?)tmdbTvShowAppends.EpisodeRunTime?.Average()
            : 0;
        FirstAirDate = tmdbTvShowAppends.FirstAirDate;
        HaveEpisodes = 0;
        Homepage = tmdbTvShowAppends.Homepage?.ToString();
        ImdbId = tmdbTvShowAppends.ExternalIds.ImdbId;
        InProduction = tmdbTvShowAppends.InProduction;
        LastEpisodeToAir = tmdbTvShowAppends.LastEpisodeToAir?.Id;
        NextEpisodeToAir = tmdbTvShowAppends.NextEpisodeToAir?.Id;
        NumberOfEpisodes = tmdbTvShowAppends.NumberOfEpisodes;
        NumberOfSeasons = tmdbTvShowAppends.NumberOfSeasons;
        OriginCountry = tmdbTvShowAppends.OriginCountry.Length > 0 ? tmdbTvShowAppends.OriginCountry[0] : null;
        OriginalLanguage = tmdbTvShowAppends.OriginalLanguage;
        Overview = tmdbTvShowAppends.Overview;
        Popularity = tmdbTvShowAppends.Popularity;
        Poster = tmdbTvShowAppends.PosterPath;
        SpokenLanguages = tmdbTvShowAppends.SpokenLanguages.Length > 0 ? tmdbTvShowAppends.SpokenLanguages[0].Name : null;
        Status = tmdbTvShowAppends.Status;
        Tagline = tmdbTvShowAppends.Tagline;
        Title = tmdbTvShowAppends.Name;
        TitleSort = tmdbTvShowAppends.Name.TitleSort(tmdbTvShowAppends.FirstAirDate);
        Trailer = tmdbTvShowAppends.Videos.Results.Length > 0 ? tmdbTvShowAppends.Videos.Results[0].Key : null;
        TvdbId = tmdbTvShowAppends.ExternalIds.TvdbId;
        Type = tmdbTvShowAppends.Type;
        VoteAverage = tmdbTvShowAppends.VoteAverage;
        VoteCount = tmdbTvShowAppends.VoteCount;

        Folder = folder;
        LibraryId = libraryId;
        MediaType = mediaType;
    }
}