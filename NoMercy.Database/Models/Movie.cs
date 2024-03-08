using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Movie : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")] public string Title { get; set; } = string.Empty;
        [JsonProperty("title_sort")] public string TitleSort { get; set; }
        [JsonProperty("duration")] public int? Duration { get; set; }
        [JsonProperty("show")] public bool Show { get; set; }
        [JsonProperty("folder")] public string? Folder { get; set; }
        [JsonProperty("adult")] public bool Adult { get; set; }
        [JsonProperty("backdrop")] public string? Backdrop { get; set; }
        [JsonProperty("budget")] public int? Budget { get; set; }
        [JsonProperty("homepage")] public string? Homepage { get; set; }
        [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
        [JsonProperty("original_title")] public string? OriginalTitle { get; set; }
        [JsonProperty("original_language")] public string? OriginalLanguage { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }
        [JsonProperty("popularity")] public double? Popularity { get; set; }
        [JsonProperty("poster")] public string? Poster { get; set; }
        [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
        [JsonProperty("revenue")] public long? Revenue { get; set; }
        [JsonProperty("runtime")] public int? Runtime { get; set; }
        [JsonProperty("status")] public string? Status { get; set; }
        [JsonProperty("tagline")] public string? Tagline { get; set; }
        [JsonProperty("trailer")] public string? Trailer { get; set; }
        [JsonProperty("video")] public string? Video { get; set; }
        [JsonProperty("vote_average")] public double? VoteAverage { get; set; }
        [JsonProperty("vote_count")] public int? VoteCount { get; set; }

        [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
        public virtual Library Library { get; set; }

        [JsonProperty("alternative_titles")]
        public virtual ICollection<AlternativeTitle> AlternativeTitles { get; set; }

        [JsonProperty("cast")] public virtual ICollection<Cast> Cast { get; set; }

        [JsonProperty("certifications")]
        public virtual ICollection<CertificationMovie> CertificationMovies { get; set; }
        
        [JsonProperty("crew")] 
        public virtual ICollection<Crew> Crew { get; set; }
        
        [JsonProperty("genre")] 
        public virtual ICollection<GenreMovie> GenreMovies { get; set; }
        
        [JsonProperty("keywords")] 
        public virtual ICollection<KeywordMovie> KeywordMovies { get; set; }
        
        [JsonProperty("media")] 
        public virtual ICollection<Media> Media { get; set; }
        
        [JsonProperty("images")] 
        public virtual ICollection<Image> Images { get; set; }
        
        [JsonProperty("seasons")] 
        public virtual ICollection<Season> Seasons { get; set; }
        
        [JsonProperty("translations")] 
        public virtual ICollection<Translation> Translations { get; set; }
        
        [JsonProperty("user_data")] 
        public virtual ICollection<UserData> UserData { get; set; }
        
        [InverseProperty("MovieFrom")] 
        public virtual ICollection<Recommendation> RecommendationFrom { get; set; }
        
        [InverseProperty("MovieTo")] 
        public virtual ICollection<Recommendation> RecommendationTo { get; set; }
        
        [InverseProperty("MovieFrom")] 
        public virtual ICollection<Similar> SimilarFrom { get; set; }
        
        [InverseProperty("MovieTo")] 
        public virtual ICollection<Similar> SimilarTo { get; set; }
        
        [JsonProperty("movie_user")] 
        public virtual ICollection<MovieUser> MovieUser { get; set; }
        
        [JsonProperty("video_files")] 
        public virtual ICollection<VideoFile> VideoFiles { get; set; } = new HashSet<VideoFile>();

        public Movie(MovieAppends movie, Ulid libraryId, string folder)
        {
            Id = movie.Id;
            Title = movie.Title;
            TitleSort = movie.Title.TitleSort(movie.ReleaseDate) ?? string.Empty;
            Duration = movie.Runtime;
            Folder = folder;
            Adult = movie.Adult;
            Backdrop = movie.BackdropPath;
            Budget = movie.Budget;
            Homepage = movie.Homepage?.ToString();
            ImdbId = movie.ImdbId;
            OriginalTitle = movie.OriginalTitle;
            OriginalLanguage = movie.OriginalLanguage;
            Overview = movie.Overview;
            Popularity = movie.Popularity;
            Poster = movie.PosterPath;
            ReleaseDate = movie.ReleaseDate;
            Revenue = movie.Revenue;
            Runtime = movie.Runtime;
            Status = movie.Status;
            Tagline = movie.Tagline;
            Trailer = movie.Video;
            Video = movie.Video;
            VoteAverage = movie.VoteAverage;
            VoteCount = movie.VoteCount;

            LibraryId = libraryId;
        }

        public Movie()
        {
        }

        public Movie(Providers.TMDB.Models.Movies.Movie input, Ulid libraryId)
        {
            Id = input.Id;
            Title = input.Title;
            TitleSort = input.Title.TitleSort(input.ReleaseDate);
            Adult = input.Adult;
            Backdrop = input.BackdropPath;
            OriginalTitle = input.OriginalTitle;
            OriginalLanguage = input.OriginalLanguage;
            Overview = input.Overview;
            Popularity = input.Popularity;
            Poster = input.PosterPath;
            ReleaseDate = input.ReleaseDate;
            Tagline = input.Tagline;
            VoteAverage = input.VoteAverage;
            VoteCount = input.VoteCount;

            LibraryId = libraryId;
        }
    }
}