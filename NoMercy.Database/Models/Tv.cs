using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Tv: ColorPaletteTimeStamps
    {
	    [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string TitleSort { get; set; } = string.Empty;
		public int? HaveEpisodes { get; set; }
		public string? Folder { get; set; }
		public string? Backdrop { get; set; }
		public int? Duration { get; set; }
		public DateTime? FirstAirDate { get; set; }
		public string? Homepage { get; set; }
		public string? ImdbId { get; set; }
		public bool? InProduction { get; set; }
		public int? LastEpisodeToAir { get; set; }
		public string? MediaType { get; set; }
		public int? NextEpisodeToAir { get; set; }
		public int? NumberOfEpisodes { get; set; }
		public int? NumberOfSeasons { get; set; }
		public string? OriginCountry { get; set; }
		public string? OriginalLanguage { get; set; }
		public string? Overview { get; set; }
		public double? Popularity { get; set; }
		public string? Poster { get; set; }
		public string? SpokenLanguages { get; set; }
		public string? Status { get; set; }
		public string? Tagline { get; set; }
		public string? Trailer { get; set; }
		public int? TvdbId { get; set; }
		public string? Type { get; set; }
		public double? VoteAverage { get; set; }
		public int? VoteCount { get; set; }
		
		public string LibraryId { get; set; } = string.Empty;
		
		public virtual Library Library { get; } = null!;
		
		public virtual ICollection<AlternativeTitle>? AlternativeTitles { get; } = null!;
		public virtual ICollection<Cast>? Cast { get; } = null!;
		public virtual ICollection<Certification_Tv>? Certification_Tvs { get; } = null!;
		public virtual ICollection<Crew>? Crew { get; } = null!;
		public virtual ICollection<Genre_Tv>? Genre_Tvs { get; } = null!;
		public virtual ICollection<Keyword_Tv>? Keyword_Tvs { get; } = null!;
		public virtual ICollection<Media>? Media { get; } = null!;
		public virtual ICollection<Image>? Images { get; } = null!;
		public virtual ICollection<Recommendation>? Recommendation_From { get; } = null!;
		public virtual ICollection<Recommendation>? Recommendation_To { get; } = null!;
		public virtual ICollection<Season>? Seasons { get; } = null!;
		
		public virtual ICollection<Similar>? Similar_From { get; } = null!;
		public virtual ICollection<Similar>? Similar_To { get; } = null!;
		public virtual ICollection<Translation>? Translations { get; set; } = null!;
		public virtual ICollection<UserData>? UserData { get; set; } = null!;
		public virtual ICollection<Episode>? Episodes { get; set; } = null!;
		
		
		public Tv()
		{ }
		
		public Tv(TvShowAppends tvShowAppends, string libraryId, string folder, string mediaType)
		{
			Id = tvShowAppends.Id;
			Backdrop = tvShowAppends.BackdropPath;
			Duration = tvShowAppends.EpisodeRunTime.Length > 0 ? tvShowAppends.EpisodeRunTime[0] : 0;
			FirstAirDate = tvShowAppends.FirstAirDate;
			HaveEpisodes = 0;
			Homepage = tvShowAppends.Homepage?.ToString();
			ImdbId = tvShowAppends.ExternalIds?.ImdbId;
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
			TitleSort = tvShowAppends.Name;
			Trailer = tvShowAppends.Videos?.Results.Length > 0 ? tvShowAppends.Videos?.Results[0].Key : null;
			TvdbId = tvShowAppends.ExternalIds?.TvdbId;
			Type = tvShowAppends.Type;
			VoteAverage = tvShowAppends.VoteAverage;
			VoteCount = tvShowAppends.VoteCount;
			
			Folder = folder;
			LibraryId = libraryId;
			MediaType = mediaType;
		}
		
        
    }
}