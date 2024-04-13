#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using Episode = NoMercy.Database.Models.Episode;
using Movie = NoMercy.Database.Models.Movie;
using Season = NoMercy.Database.Models.Season;


namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class InfoResponseDto
{
    [JsonProperty("data")] public InfoResponseItemDto? Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, int, string, Task<Tv?>> GetTv =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language) =>
            mediaContext.Tvs.AsNoTracking()
                
                .Where(tv => tv.Id == id)
                .Where(tv => tv.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
                
                .Include(tv => tv.TvUser)
                .Include(tv => tv.Library)
                    .ThenInclude(library => library.LibraryUsers)
                
                .Include(tv => tv.Media)
                .Include(tv => tv.AlternativeTitles)

                .Include(tv => tv.Translations
                    .Where(translation => translation.Iso6391 == language))

                .Include(tv => tv.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )

                .Include(tv => tv.CertificationTvs)
                    .ThenInclude(certificationTv => certificationTv.Certification)

                .Include(tv => tv.GenreTvs)
                    .ThenInclude(genreTv => genreTv.Genre)

                .Include(tv => tv.KeywordTvs)
                    .ThenInclude(keywordTv => keywordTv.Keyword)

                .Include(tv => tv.Cast)
                    .ThenInclude(castTv => castTv.Person)

                .Include(tv => tv.Cast)
                    .ThenInclude(castTv => castTv.Role)

                .Include(tv => tv.Crew)
                    .ThenInclude(crewTv => crewTv.Person)

                .Include(tv => tv.Crew)
                    .ThenInclude(crewTv => crewTv.Job)

                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Translations
                    .Where(translation => translation.Iso6391 == language)
                )

                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(episode => episode.Translations
                            .Where(translation => translation.Iso6391 == language)
                )

                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(episode => episode.VideoFiles)
                            .ThenInclude(file => file.UserData)

                .Include(tv => tv.Episodes)
                    .ThenInclude(episode => episode.VideoFiles)
                        .ThenInclude(file => file.UserData)

                .Include(tv => tv.RecommendationFrom)
                .Include(tv => tv.SimilarFrom)

                .FirstOrDefault());
    
    public static readonly Func<MediaContext, Guid, int, Task<Tv?>> GetTvAvailable =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id) =>
            mediaContext.Tvs.AsNoTracking()
                
                .Where(tv => tv.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
                .Where(tv => tv.Id == id)
                
                .Include(tv => tv.Episodes)
                    .ThenInclude(tv => tv.VideoFiles)
                
                .FirstOrDefault());
            
    public static readonly Func<MediaContext, Guid, int, string, Task<Tv?>> GetTvPlaylist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language) =>
            mediaContext.Tvs.AsNoTracking()
                .Where(tv => tv.Id == id)
            
                .Where(tv => tv.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
            
                .Include(tv => tv.Seasons.OrderBy(season => season.SeasonNumber))
                    .ThenInclude(season => season.Episodes.OrderBy(episode => episode.EpisodeNumber))
            
                .Include(tv => tv.Translations
                    .Where(translation => translation.Iso6391 == language))
                
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(tv => tv.Tv)
                            .ThenInclude(tv => tv.Translations
                                .Where(translation => translation.Iso6391 == language))
                
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(tv => tv.Tv)
                            .ThenInclude(tv => tv.Media
                                .Where(media => media.Type == "video"))
            
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(tv => tv.Tv)
                            .ThenInclude(tv => tv.Images
                                .Where(image => image.Type == "logo"))
                
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(tv => tv.VideoFiles)
                            .ThenInclude(file => file.UserData)
                
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Translations
                        .Where(translation => translation.Iso6391 == language))
            
                .Include(tv => tv.Seasons)
                    .ThenInclude(season => season.Episodes)
                        .ThenInclude(episode => episode.Translations
                            .Where(translation => translation.Iso6391 == language)
                        )

                .FirstOrDefault());
    
    public static readonly Func<MediaContext, Guid, int, string, Task<Movie?>> GetMovie =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language) =>
            mediaContext.Movies.AsNoTracking()
            .Where(movie => movie.Id == id)
            
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Include(movie => movie.MovieUser
                .Where(movieUser => movieUser.UserId == userId)
            )
            
            .Include(movie => movie.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(movie => movie.Media)
            .Include(movie => movie.AlternativeTitles)
            
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == language))
            
            .Include(movie => movie.Images
                .Where(image => 
                    (image.Type == "logo" && image.Iso6391 == "en")
                    || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                ))
            
            .Include(movie => movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
            
            .Include(movie => movie.KeywordMovies)
                .ThenInclude(keywordMovie => keywordMovie.Keyword)
            
            .Include(movie => movie.Cast.Where(cast => cast.Role!.Character != null))
                .ThenInclude(castMovie => castMovie.Person)
            
            .Include(movie => movie.Cast.Where(cast => cast.Role!.Character != null))
                .ThenInclude(castMovie => castMovie.Role)
            
            .Include(movie => movie.Crew.Where(cast => cast.Job!.Task != null))
                .ThenInclude(crewMovie => crewMovie.Person)
            
            .Include(movie => movie.Crew.Where(cast => cast.Job!.Task != null))
                .ThenInclude(crewMovie => crewMovie.Job)
            
            .Include(movie => movie.RecommendationFrom)
            
            .Include(movie => movie.SimilarFrom)
            
            .Include(movie => movie.VideoFiles)
                .ThenInclude(file => file.UserData)
            
            .FirstOrDefault());
    
    public static readonly Func<MediaContext, Guid, int, Task<Movie?>> GetMovieAvailable =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id) =>
            mediaContext.Movies.AsNoTracking()
                
        .Where(movie => movie.Library.LibraryUsers
        .FirstOrDefault(u => u.UserId == userId) != null)
            
        .Where(movie => movie.Id == id)
        .Include(movie => movie.VideoFiles)
            .FirstOrDefault());
    
    public static readonly Func<MediaContext, Guid, int, string, Task<Movie?>> GetMoviePlaylist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language) =>
            mediaContext.Movies.AsNoTracking()
            
                .Where(movie => movie.Id == id)
            
                .Where(movie => movie.Library.LibraryUsers
                    .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            
                .Include(movie => movie.Media
                    .Where(media => media.Type == "video"))
            
                .Include(movie => movie.Images
                    .Where(image => image.Type == "logo"))
            
                .Include(movie => movie.Translations
                    .Where(translation => translation.Iso6391 == language))
            
                .Include(movie => movie.VideoFiles)
                .ThenInclude(file => file.UserData)
            
                .FirstOrDefault());
}

public class InfoResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }
    [JsonProperty("backdrops")] public ImageDto[] Backdrops { get; set; }
    [JsonProperty("posters")] public ImageDto[] Posters { get; set; }
    [JsonProperty("similar")] public RelatedDto[] Similar { get; set; }
    [JsonProperty("recommendations")] public RelatedDto[] Recommendations { get; set; }
    [JsonProperty("cast")] public PeopleDto[] Cast { get; set; }
    [JsonProperty("crew")] public PeopleDto[] Crew { get; set; }
    [JsonProperty("contentRatings")] public ContentRating[]? ContentRatings { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("duration")] public double Duration { get; set; }
    [JsonProperty("number_of_episodes")] public int NumberOfEpisodes { get; set; }
    [JsonProperty("have_episodes")] public int? HaveEpisodes { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("voteAverage")] public double VoteAverage { get; set; }
    [JsonProperty("externalIds")] public ExternalIds? ExternalIds { get; set; }
    [JsonProperty("creators")] public DirectorDto[] Creators { get; set; }
    [JsonProperty("directors")] public DirectorDto[] Directors { get; set; }
    [JsonProperty("writers")] public DirectorDto[] Writers { get; set; }
    [JsonProperty("director")] public DirectorDto? Director { get; set; }
    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }
    [JsonProperty("keywords")] public string[] Keywords { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }
    [JsonProperty("seasons")] public SeasonDto[] Seasons { get; set; }
    
    public InfoResponseItemDto(Movie movie, string? country)
    {
        string? title = movie.Translations.FirstOrDefault()?.Title;
        string? overview = movie.Translations.FirstOrDefault()?.Overview;

        Id = movie.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : movie.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : movie.Overview;
        Type = "movie";
        MediaType = "movie";
        
        Watched = movie.VideoFiles
            .Any(videoFile => videoFile.UserData.Any());
        
        Favorite = movie.MovieUser.Count != 0;
        
        TitleSort = movie.Title.TitleSort(movie.ReleaseDate);
        
        Duration = movie.VideoFiles.Count != 0
            ? movie.VideoFiles.Select(videoFile => videoFile.Duration?.ToSeconds() ?? 0).Average() / 60
            : movie.Duration ?? 0;
        
        Year = movie.ReleaseDate.ParseYear();
        VoteAverage = movie.VoteAverage ?? 0;

        ColorPalette = movie.ColorPalette;
        Backdrop = movie.Backdrop;
        Poster = movie.Poster;
        
        Translations = movie.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        ContentRatings = movie.CertificationMovies
            .Where(certificationMovie => certificationMovie.Certification.Iso31661 == "US" || certificationMovie.Certification.Iso31661 == country)
            .Select(certificationMovie => new ContentRating
            {
                Rating = certificationMovie.Certification.Rating,
                Iso31661 = certificationMovie.Certification.Iso31661
            })
            .ToArray();
        
        Creators = movie.Crew
            .Where(crew => crew.Job?.Task == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Directors = movie.Crew
        //     .Where(crew => crew.Job.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .ToArray();
        
        Writers = movie.Crew
            .Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Director = movie.Crew.Where(crew => crew.Job?.Task == "Director") 
            .Select(crew => new DirectorDto(crew))
            .FirstOrDefault();
        
        Keywords = movie.KeywordMovies?
            .Select(keywordMovie => keywordMovie.Keyword.Name)
            .ToArray() ?? [];

        Logo = movie.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Videos = movie.Media?
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray() ?? [];

        Backdrops = movie.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = movie.Images
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = movie.GenreMovies?
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray() ?? [];

        Cast = movie.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = movie.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = movie.SimilarFrom?
            .Select(similar => new RelatedDto(similar, "movie"))
            .ToArray() ?? [];
        
        Recommendations = movie.RecommendationFrom?
            .Select(recommendation => new RelatedDto(recommendation, "movie"))
            .ToArray() ?? [];
    }

    public InfoResponseItemDto(MovieAppends movie, string? country)
    {
        string? title = movie.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Title;

        string? overview = movie.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Overview;

        Id = movie.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : movie.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : movie.Overview;
        Type = "movie";
        MediaType = "movie";
        
        Watched = false;
        
        Favorite = false;
        
        TitleSort = movie.Title.TitleSort(movie.ReleaseDate);
        
        Duration = movie.Runtime;
        
        Year = movie.ReleaseDate.ParseYear();
        VoteAverage = movie.VoteAverage;

        ColorPalette = new IColorPalettes();
        Backdrop = movie.BackdropPath;
        Poster = movie.PosterPath;
        
        Translations = movie.Translations.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        // ContentRatings = movie.ReleaseDates.Results
        //     .Where(certificationMovie => certificationMovie.Iso31661 == "US" || certificationMovie.Iso31661 == country)
        //     .Select(certificationMovie => new ContentRatings
        //     {
        //         Rating = certificationMovie.ReleaseDates
        //             .First(cert => cert.Iso6391 == "US" || cert.Iso6391 == country).Certification,
        //         Iso31661 = certificationMovie.Iso31661
        //     })
        //     .ToArray();
        //
        Creators = movie.Credits.Crew
            .Where(crew => crew.Job == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Directors = movie.Credits.Crew
        //     .Where(crew => crew.Job.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .ToArray();
        
        Writers = movie.Credits.Crew
            .Where(crew => crew.Job == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Director = movie.Credits.Crew.Where(crew => crew.Job == "Director") 
            .Select(crew => new DirectorDto(crew))
            .FirstOrDefault();
        
        Keywords = movie.Keywords.Results
            .Select(keywordMovie => keywordMovie.Name)
            .ToArray();

        Logo = movie.Images.Logos
            .FirstOrDefault(logo => logo.Iso6391 == "en")?.FilePath;

        Videos = movie.Videos.Results
            .Select(media => new VideoDto(media))
            .ToArray();

        Backdrops = movie.Images.Backdrops
            .Where(image => image.Iso6391 is "en" or null)
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = movie.Images.Posters
            .Where(image => image.Iso6391 is "en" or null)
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = movie.Genres
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();

        Cast = movie.Credits.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = movie.Credits.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = movie.Similar.Results
            .Select(similar => new RelatedDto(similar, "movie"))
            .ToArray();
        
        Recommendations = movie.Recommendations.Results
            .Select(recommendation => new RelatedDto(recommendation, "movie"))
            .ToArray();
    }
    
    public InfoResponseItemDto(Tv tv, string? country)
    {
        string? title = tv.Translations.FirstOrDefault()?.Title;
        string? overview = tv.Translations.FirstOrDefault()?.Overview;

        Id = tv.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : tv.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : tv.Overview;
        Type = tv.Type ?? "tv";
        MediaType = "tv";
        
        Watched = tv.Episodes
            .Any(episode => episode.VideoFiles
                .Any(videoFile => videoFile.UserData.Any()));
        
        Favorite = tv.TvUser.Count != 0;
        
        TitleSort = tv.Title.TitleSort(tv.FirstAirDate);
        
        Translations = tv.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        Duration = tv.Episodes.Any(episode => episode.VideoFiles.Count > 0)
            ? tv.Episodes
                .SelectMany(episode => episode.VideoFiles)
                .Select(videoFile => (videoFile.Duration?.ToSeconds() ?? 0) / 60).Average() 
            : tv.Duration ?? 0;
        
        NumberOfEpisodes = tv.NumberOfEpisodes;
        HaveEpisodes = tv.Episodes
            .SelectMany(episode => episode.VideoFiles.Where(videoFile => videoFile.UserData.Any()))
            .Count();
        
        Year = tv.FirstAirDate.ParseYear();
        VoteAverage = tv.VoteAverage ?? 0;

        ColorPalette = tv.ColorPalette;
        Backdrop = tv.Backdrop;
        Poster = tv.Poster;

        ContentRatings = tv.CertificationTvs
            .Where(certificationMovie => certificationMovie.Certification.Iso31661 == "US" || certificationMovie.Certification.Iso31661 == country)
            .Select(certificationTv => new ContentRating
            {
                Rating = certificationTv.Certification.Rating,
                Iso31661 = certificationTv.Certification.Iso31661
            })
            .ToArray();

        Creators = tv.Crew
            .Where(crew => crew.Job?.Task == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Directors = tv.Crew
        //     .Where(crew => crew.Job.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .ToArray();
        
        Writers = tv.Crew
            .Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Director = tv.Crew.Where(crew => crew.Job.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = tv.KeywordTvs?
            .Select(keywordTv => keywordTv.Keyword.Name)
            .ToArray() ?? [];

        Logo = tv.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Videos = tv.Media?
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray() ?? [];

        Backdrops = tv.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = tv.Images
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = tv.GenreTvs?
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray() ?? [];
        
        Cast = tv.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = tv.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = tv.SimilarFrom?
            .Select(similar => new RelatedDto(similar, "tv"))
            .ToArray() ?? [];
        
        Recommendations = tv.RecommendationFrom?
            .Select(recommendation => new RelatedDto(recommendation, "tv"))
            .ToArray() ?? [];

        Seasons = tv.Seasons?
            .OrderBy(season => season.SeasonNumber)
            .Select(season => new SeasonDto(season))
            .ToArray() ?? [];
    }
    
    public InfoResponseItemDto(TvShowAppends tv, string? country)
    {
        string? title = tv.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Title;

        string? overview = tv.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Overview;

        Id = tv.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : tv.Name;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : tv.Overview;
        Type = tv.Type ?? "tv";
        MediaType = "tv";
        
        Watched = false;
        Favorite = false;
        
        TitleSort = tv.Name.TitleSort(tv.FirstAirDate);
        
        Translations = tv.Translations.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        Duration = tv.EpisodeRunTime?.Length > 0
            ? tv.EpisodeRunTime.Average()
            : 0;
        
        NumberOfEpisodes = tv.NumberOfEpisodes;
        HaveEpisodes = 0;
        Year = tv.FirstAirDate.ParseYear();
        VoteAverage = tv.VoteAverage;

        // ColorPalette = tv.ColorPalette;
        Backdrop = tv.BackdropPath;
        Poster = tv.PosterPath;

        ContentRatings = tv.ContentRatings.Results
            .Where(certificationMovie => certificationMovie.Iso31661 == "US" || certificationMovie.Iso31661 == country)
            .Select(certificationTv => new ContentRating
            {
                Rating = certificationTv.Rating,
                Iso31661 = certificationTv.Iso31661
            })
            .ToArray();

        Creators = tv.Credits.Crew
            .Where(crew => crew.Job == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Directors = tv.Credits.Crew
            .Where(crew => crew.Job == "Director") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Writers = tv.Credits.Crew
            .Where(crew => crew.Job == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Director = tv.Credits.Crew?.Where(crew => crew.Job == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = tv.Keywords.Results
            .Select(keywordTv => keywordTv.Name)
            .ToArray();

        Logo = tv.Images.Logos
            .FirstOrDefault(media => media.Iso6391 == "en")?.FilePath;

        Videos = tv.Videos.Results
            .Select(media => new VideoDto(media))
            .ToArray();

        Backdrops = tv.Images.Backdrops
            .Where(image => image.Iso6391 is "en" or null)
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = tv.Images.Posters
            .Where(image => image.Iso6391 is "en" or null)
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = tv.Genres
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        
        Cast = tv.Credits.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = tv.Credits.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = tv.Similar.Results
            .Select(similar => new RelatedDto(similar, "tv"))
            .ToArray();
        
        Recommendations = tv.Recommendations.Results
            .Select(recommendation => new RelatedDto(recommendation, "tv"))
            .ToArray();
        //
        // Seasons = tv.Seasons
        //     .OrderBy(season => season.SeasonNumber)
        //     .Select(season => new SeasonDto(tv.Id, season, country))
        //     .ToArray();
        Seasons = [];
    }

    public InfoResponseItemDto(Collection collection, string country)
    {
        string? title = collection.Translations
            .FirstOrDefault()?.Title;

        string? overview = collection.Translations
            .FirstOrDefault()?.Overview;

        Id = collection.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : collection.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : collection.Overview;
        Type = "collection";
        MediaType = "collection";
        // Watched = tv.Watched;
        // Favorite = tv.Favorite;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate
                .ParseYear());
        
        Translations = collection.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        Year = collection.CollectionMovies
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate
            .ParseYear() ?? 0;
        
        VoteAverage = collection.CollectionMovies
            .Average(collectionMovie => collectionMovie.Movie.VoteAverage) ?? 0;

        ColorPalette = collection.ColorPalette;
        Backdrop = collection.Backdrop;
        Poster = collection.Poster;
        
        ContentRatings = collection.CollectionMovies
            .Select(certificationMovie => new ContentRating
            {
                Rating = certificationMovie.Movie.CertificationMovies
                    .First(cert => cert.Certification.Iso31661 == "US" || cert.Certification.Iso31661 == country).Certification.Rating,
                Iso31661 = certificationMovie.Movie.CertificationMovies
                    .First(cert => cert.Certification.Iso31661 == "US" || cert.Certification.Iso31661 == country).Certification.Iso31661
            })
            .ToArray();

        Creators = collection.CollectionMovies
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew
               .FirstOrDefault(crew => crew.Job?.Task == "Creator") ?? new Crew()))
            .ToArray();
        
        Directors = collection.CollectionMovies
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew
               .FirstOrDefault(crew => crew.Job?.Task == "Director") ?? new Crew()))
            .ToArray();
        
        Writers = collection.CollectionMovies
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew
               .FirstOrDefault(crew => crew.Job?.Task == "Writer") ?? new Crew()))
            .ToArray();
        
        // Director = collection.Movies?
        //     .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew?
        //         .FirstOrDefault(crew => crew.Job.Task == "Director") ?? new Crew()))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = collection.CollectionMovies
            .SelectMany(collectionMovie => collectionMovie.Movie.KeywordMovies)
            .Select(keywordMovie => keywordMovie.Keyword.Name)
            .ToArray();

        Logo = collection.CollectionMovies
            .Select(collectionMovie => collectionMovie.Movie.Images
               .FirstOrDefault(media => media.Type == "logo")?.FilePath)
            .FirstOrDefault();
    }

}

public class ImageDto
{
    [JsonProperty("height")] public long Height { get; set; }
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("src")] public string? Src { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("width")] public long Width { get; set; }
    [JsonProperty("voteAverage")] public double VoteAverage { get; set; }
    [JsonProperty("voteCount")] public long VoteCount { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    public ImageDto(Image media)
    {
        Id = media.Id;
        Src = media.FilePath;
        Width = media.Width ?? 0;
        Type = media.Type;
        Height = media.Height ?? 0;
        VoteAverage = media.VoteAverage ?? 0;
        VoteCount = media.VoteCount ?? 0;
        ColorPalette = media.ColorPalette;
    }

    public ImageDto(Providers.TMDB.Models.Shared.Image media)
    {
        Src = media.FilePath;
        Width = media.Width;
        Height = media.Height;
        VoteAverage = media.VoteAverage;
        VoteCount = media.VoteCount;
        ColorPalette = new IColorPalettes();
    }

    public ImageDto(Providers.TMDB.Models.Shared.Profile image)
    {
        Src = image.FilePath;
        Width = image.Width;
        Height = image.Height;
        VoteAverage = 0;
        VoteCount = 0;
        ColorPalette = new IColorPalettes();
    }
}

public class DirectorDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    
    public DirectorDto(Crew crew)
    {
        Id = crew.Person.Id;
        Name = crew.Person.Name;
        ColorPalette = crew.Person.ColorPalette;
    }

    public DirectorDto(Providers.TMDB.Models.Shared.Crew crew)
    {
        Id = crew.Id;
        Name = crew.Name;
        ColorPalette = new IColorPalettes();
    }
}

public class ExternalIds
{
    [JsonProperty("imdbId")] public string ImdbId { get; set; }
    [JsonProperty("tvdbId")] public long TvdbId { get; set; }
}

public class RelatedDto
{
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("number_of_episodes")] public int? NumberOfEpisodes { get; set; }
    [JsonProperty("have_episodes")] public int? HaveEpisodes { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    
    public RelatedDto(Recommendation recommendation, string type)
    {
        Id = recommendation.MediaId;
        Overview = recommendation.Overview;
        Poster = recommendation.Poster;
        Title = recommendation.Title;
        TitleSort = recommendation.TitleSort;
        MediaType = type;
        ColorPalette = recommendation.ColorPalette;
        NumberOfEpisodes = type == "tv" ? recommendation.TvFrom.NumberOfEpisodes : null;
        HaveEpisodes = type == "tv" ? recommendation.TvFrom.HaveEpisodes : null;
    }

    public RelatedDto(RecommendationsMovie recommendation, string type)
    {
        Id = recommendation.Id;
        Overview = recommendation.Overview;
        Poster = recommendation.PosterPath;
        Title = recommendation.Title;
        TitleSort = recommendation.Title.TitleSort(recommendation.ReleaseDate);
        MediaType = type;
        ColorPalette = new IColorPalettes();
        NumberOfEpisodes = 0;
        HaveEpisodes = 0;
    }
    
    public RelatedDto(RecommendationsTvShow recommendation, string type)
    {
        Id = recommendation.Id;
        Overview = recommendation.Overview;
        Poster = recommendation.PosterPath;
        Title = recommendation.Name;
        TitleSort = recommendation.Name.TitleSort();
        MediaType = type;
        ColorPalette = new IColorPalettes();
        NumberOfEpisodes = 0;
        HaveEpisodes = 0;
    }

    public RelatedDto(Similar similar, string type)
    {
        Id = similar.MediaId;
        Overview = similar.Overview;
        Poster = similar.Poster;
        Title = similar.Title;
        TitleSort = similar.TitleSort;
        MediaType = type;
        ColorPalette = similar.ColorPalette;
        NumberOfEpisodes = type == "tv" ? similar.TvFrom.NumberOfEpisodes : null;
        HaveEpisodes = type == "tv" ? similar.TvFrom.HaveEpisodes : null;
    }

    public RelatedDto(SimilarMovie similar, string type)
    {
        Id = similar.Id;
        Overview = similar.Overview;
        Poster = similar.PosterPath;
        Title = similar.Title;
        TitleSort = similar.Title.TitleSort(similar.ReleaseDate);
        MediaType = type;
        ColorPalette = new IColorPalettes();
        NumberOfEpisodes = 0;
        HaveEpisodes = 0;
    }

    public RelatedDto(SimilarTvShow recommendation, string type)
    {
        Id = recommendation.Id;
        Overview = recommendation.Overview;
        Poster = recommendation.PosterPath;
        Title = recommendation.Name;
        TitleSort = recommendation.Name.TitleSort();
        MediaType = type;
        ColorPalette = new IColorPalettes();
        NumberOfEpisodes = 0;
        HaveEpisodes = 0;
    }

}

public class RecommendationColorPaletteDto
{
    [JsonProperty("poster")] public IColorPalettes Poster { get; set; }

    [JsonProperty("backdrop")] public IColorPalettes Backdrop { get; set; }
}

public class SeasonDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("seasonNumber")] public long SeasonNumber { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("episodes")] public EpisodeDto[] Episodes { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }

    public SeasonDto(Season season)
    {
        string? title = season.Translations.FirstOrDefault()?.Title;
        string? overview = season.Translations.FirstOrDefault()?.Overview;
        
        Id = season.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : season.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : season.Overview;
        Poster = season.Poster;
        SeasonNumber = season.SeasonNumber;
        ColorPalette = season.ColorPalette;
        Translations = season.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        Episodes = season.Episodes
            .OrderBy(episode => episode.EpisodeNumber)
            .Select(episode => new EpisodeDto(episode))
            .ToArray();
    }

    public SeasonDto(int tvId, Providers.TMDB.Models.Season.Season season, string country)
    {
        SeasonClient seasonClient = new(tvId, season.SeasonNumber);
        SeasonAppends? seasonData = seasonClient.WithAllAppends().Result;

        string? title = seasonData?.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Title;
        
        string? overview = seasonData?.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Overview;
        
        Id = season.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : season.Name;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : season.Overview;
        Poster = season.PosterPath;
        SeasonNumber = season.SeasonNumber;
        ColorPalette = new IColorPalettes();
        Translations = seasonData?.Translations.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray() ?? [];
        Episodes = seasonData?.Episodes
            .OrderBy(episode => episode.EpisodeNumber)
            .Select(episode => new EpisodeDto(tvId, season.SeasonNumber, episode.EpisodeNumber, country))
            .ToArray() ?? [];
    }
}

public class EpisodeDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("episodeNumber")] public long EpisodeNumber { get; set; }
    [JsonProperty("seasonNumber")] public long SeasonNumber { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("airDate")] public DateTime? AirDate { get; set; }
    [JsonProperty("still")] public string? Still { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("progress")] public object? Progress { get; set; }
    [JsonProperty("available")] public bool Available { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }

    public EpisodeDto(Episode episode)
    {
        string? title = episode.Translations.FirstOrDefault()?.Title;
        string? overview = episode.Translations.FirstOrDefault()?.Overview;
        
        Id = episode.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : episode.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : episode.Overview;
        EpisodeNumber = episode.EpisodeNumber;
        SeasonNumber = episode.SeasonNumber;
        AirDate = episode.AirDate;
        Still = episode.Still;
        ColorPalette = episode.ColorPalette;
        Available = episode.VideoFiles.Count != 0;
        Translations = episode.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
    }

    public EpisodeDto(int tvId, int seasonNumber, int episodeNumber, string country)
    {
        EpisodeClient episodeClient = new(tvId, seasonNumber, episodeNumber);
        EpisodeAppends? episodeData = episodeClient.WithAllAppends().Result;

        if (episodeData is null) return;

        string? title = episodeData.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Title;

        string? overview = episodeData.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Data.Overview;
        
        Id = episodeData.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : episodeData.Name;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : episodeData.Overview;
        EpisodeNumber = episodeData.EpisodeNumber;
        SeasonNumber = episodeData.SeasonNumber;
        AirDate = episodeData.AirDate;
        Still = episodeData.StillPath;
        ColorPalette = new IColorPalettes();
        Available = false;
        Translations = episodeData.Translations.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
    }
}
