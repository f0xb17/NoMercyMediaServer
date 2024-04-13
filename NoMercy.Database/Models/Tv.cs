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
    [JsonProperty("number_of_episodes")] public int NumberOfEpisodes { get; set; }
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
    public virtual Library Library { get; set; }

    [JsonProperty("alternative_titles")]
    public virtual ICollection<AlternativeTitle> AlternativeTitles { get; set; }
    
    [JsonProperty("casts")] 
    public virtual ICollection<Cast> Cast { get; set; }
    
    [JsonProperty("certifications")] 
    public virtual ICollection<CertificationTv> CertificationTvs { get; set; }
    
    [JsonProperty("crews")] 
    public virtual ICollection<Crew> Crew { get; set; }
    
    [JsonProperty("genres")] 
    public virtual ICollection<GenreTv> GenreTvs { get; set; }
    
    [JsonProperty("keywords")] 
    public virtual ICollection<KeywordTv> KeywordTvs { get; set; }
    
    [JsonProperty("medias")] 
    public virtual ICollection<Media> Media { get; set; }
    
    [JsonProperty("images")] 
    public virtual ICollection<Image> Images { get; set; }
    
    [JsonProperty("seasons")] 
    public virtual ICollection<Season> Seasons { get; set; }
    
    [JsonProperty("translations")] 
    public virtual ICollection<Translation> Translations { get; set; }
    
    [JsonProperty("user_data")] 
    public virtual ICollection<UserData> UserData { get; set; }

    [JsonProperty("episodes")] 
    public virtual ICollection<Episode> Episodes { get; set; } = new HashSet<Episode>();
    
    [InverseProperty("TvFrom")] 
    public virtual ICollection<Recommendation> RecommendationFrom { get; set; }
    
    [InverseProperty("TvTo")] 
    public virtual ICollection<Recommendation> RecommendationTo { get; set; }
    
    [InverseProperty("TvFrom")] 
    public virtual ICollection<Similar> SimilarFrom { get; set; }
    
    [InverseProperty("TvTo")] 
    public virtual ICollection<Similar> SimilarTo { get; set; }
    
    [JsonProperty("tv_user")] 
    public virtual ICollection<TvUser> TvUser { get; set; }

    public Tv()
    {
    }

    public Tv(TvShowAppends tvShowAppends, Ulid libraryId, string folder, string mediaType)
    {
        Id = tvShowAppends.Id;
        Backdrop = tvShowAppends.BackdropPath;
        Duration = tvShowAppends.EpisodeRunTime?.Length > 0 
            ? (int?)tvShowAppends.EpisodeRunTime?.Average() 
            : 0;
        FirstAirDate = tvShowAppends.FirstAirDate;
        HaveEpisodes = 0;
        Homepage = tvShowAppends.Homepage?.ToString();
        ImdbId = tvShowAppends.ExternalIds.ImdbId;
        InProduction = tvShowAppends.InProduction;
        LastEpisodeToAir = tvShowAppends.LastEpisodeToAir?.Id;
        NextEpisodeToAir = tvShowAppends.NextEpisodeToAir?.Id;
        NumberOfEpisodes = tvShowAppends.NumberOfEpisodes;
        NumberOfSeasons = tvShowAppends.NumberOfSeasons;
        OriginCountry = tvShowAppends.OriginCountry.Length > 0 ? tvShowAppends.OriginCountry[0] : null;
        OriginalLanguage = tvShowAppends.OriginalLanguage;
        Overview = tvShowAppends.Overview;
        Popularity = tvShowAppends.Popularity;
        Poster = tvShowAppends.PosterPath;
        SpokenLanguages = tvShowAppends.SpokenLanguages.Length > 0 ? tvShowAppends.SpokenLanguages[0].Name : null;
        Status = tvShowAppends.Status;
        Tagline = tvShowAppends.Tagline;
        Title = tvShowAppends.Name;
        TitleSort = tvShowAppends.Name.TitleSort(tvShowAppends.FirstAirDate);
        Trailer = tvShowAppends.Videos.Results.Length > 0 ? tvShowAppends.Videos.Results[0].Key : null;
        TvdbId = tvShowAppends.ExternalIds.TvdbId;
        Type = tvShowAppends.Type;
        VoteAverage = tvShowAppends.VoteAverage;
        VoteCount = tvShowAppends.VoteCount;

        Folder = folder;
        LibraryId = libraryId;
        MediaType = mediaType;
    }
}
