using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class InfoResponseDto
{
    [JsonProperty("data")] public InfoResponseItemDto? Data { get; set; }
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
    [JsonProperty("numberOfEpisodes")] public int NumberOfEpisodes { get; set; }
    [JsonProperty("haveEpisodes")] public int? HaveEpisodes { get; set; }
    [JsonProperty("year")] public int Year { get; set; }
    [JsonProperty("voteAverage")] public double VoteAverage { get; set; }
    [JsonProperty("externalIds")] public ExternalIds? ExternalIds { get; set; }
    [JsonProperty("creators")] public DirectorDto[] Creators { get; set; }
    [JsonProperty("directors")] public DirectorDto[] Directors { get; set; }
    [JsonProperty("writers")] public DirectorDto[] Writers { get; set; }
    [JsonProperty("director")] public DirectorDto Director { get; set; }
    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }
    [JsonProperty("keywords")] public string[] Keywords { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }
    [JsonProperty("seasons")] public SeasonDto[] Seasons { get; set; }
    
    public InfoResponseItemDto(Movie movie, string country)
    {
        string title = movie.Translations
            .FirstOrDefault()?.Title ?? movie.Title;
        
        string overview = movie.Translations
            .FirstOrDefault()?.Overview ?? movie.Overview ?? string.Empty;

        Id = movie.Id;
        Title = title;
        Overview = overview;
        Type = "movie";
        MediaType = "movie";
        
        Watched = movie.VideoFiles
            .Any(videoFile => videoFile.UserData.FirstOrDefault()?.Played == true);
        
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
        
        Directors = movie.Crew
            .Where(crew => crew.Job?.Task == "Director") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Writers = movie.Crew
            .Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Director = movie.Crew?.Where(crew => crew.Job?.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        //
        Keywords = movie.KeywordMovies
            .Select(keywordMovie => keywordMovie.Keyword.Name)
            .ToArray();

        Logo = movie.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Videos = movie.Media
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray();

        Backdrops = movie.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = movie.Images
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();

        Cast = movie.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = movie.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = movie.SimilarFrom
            .Select(similar => new RelatedDto(similar, "movie"))
            .ToArray();
        
        Recommendations = movie.RecommendationFrom
            .Select(recommendation => new RelatedDto(recommendation, "movie"))
            .ToArray();
    }

    public InfoResponseItemDto(Tv tv, string country)
    {
        string title = tv.Translations
            .FirstOrDefault()?.Title ?? tv.Title ?? string.Empty;
        
        string overview = tv.Translations
            .FirstOrDefault()?.Overview ?? tv.Overview ?? string.Empty;

        Id = tv.Id;
        Title = title;
        Overview = overview;
        Type = tv.Type ?? "tv";
        MediaType = "tv";
        
        Watched = tv.Episodes
            .Any(episode => episode.VideoFiles
                .Any(videoFile => videoFile.UserData.FirstOrDefault()?.Played == true));
        
        Favorite = tv.TvUser.Count != 0;
        
        TitleSort = tv.Title.TitleSort(tv.FirstAirDate);
        
        Translations = tv.Translations
            .Select(translation => new TranslationDto(translation))
            .ToArray();
        
        Duration = tv.Episodes.Any(episode => episode.VideoFiles?.Count > 0)
            ? tv.Episodes
                .SelectMany(episode => episode.VideoFiles)
                .Select(videoFile => (videoFile.Duration?.ToSeconds() ?? 0) / 60).Average() 
            : tv.Duration ?? 0;
        
        NumberOfEpisodes = tv.NumberOfEpisodes;
        HaveEpisodes = tv.HaveEpisodes;
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
        
        Directors = tv.Crew
            .Where(crew => crew.Job?.Task == "Director") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        Writers = tv.Crew
            .Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray();
        
        // Director = tv.Crew?.Where(crew => crew.Job?.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = tv.KeywordTvs
            .Select(keywordTv => keywordTv.Keyword.Name)
            .ToArray();

        Logo = tv.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Videos = tv.Media
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray();

        Backdrops = tv.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = tv.Images
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray();

        Genres = tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        
        Cast = tv.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray();
        
        Crew = tv.Crew
            .Select(crew => new PeopleDto(crew))
            .ToArray();

        Similar = tv.SimilarFrom
            .Select(similar => new RelatedDto(similar, "tv"))
            .ToArray();
        
        Recommendations = tv.RecommendationFrom
            .Select(recommendation => new RelatedDto(recommendation, "tv"))
            .ToArray();

        Seasons = tv.Seasons
            .OrderBy(season => season.SeasonNumber)
            .Select(season => new SeasonDto(season))
            .ToArray();
    }

    public InfoResponseItemDto(Collection collection, string country)
    {
        string title = collection.Translations
            .FirstOrDefault()?.Title ?? collection.Title;
        
        string overview = collection.Translations
            .FirstOrDefault()?.Overview ?? collection.Overview ?? string.Empty;

        Id = collection.Id;
        Title = title;
        Overview = overview;
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
        //         .FirstOrDefault(crew => crew.Job?.Task == "Director") ?? new Crew()))
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
    [JsonProperty("width")] public long Width { get; set; }
    [JsonProperty("voteAverage")] public double VoteAverage { get; set; }
    [JsonProperty("voteCount")] public long VoteCount { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    public ImageDto(Image media)
    {
        Id = media.Id;
        Src = media.FilePath;
        Width = media.Width ?? 0;
        Height = media.Height ?? 0;
        VoteAverage = media.VoteAverage ?? 0;
        VoteCount = media.VoteCount ?? 0;
        ColorPalette = media.ColorPalette;
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
    [JsonProperty("numberOfEpisodes")] public int? NumberOfEpisodes { get; set; }
    [JsonProperty("haveEpisodes")] public int? HaveEpisodes { get; set; }

    [JsonProperty("color_palette")]
    public IColorPalettes? ColorPalette { get; set; }
    
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
        string title = season.Translations
            .FirstOrDefault()?.Title ?? season.Title ?? string.Empty;
        
        string overview = season.Translations
            .FirstOrDefault()?.Overview ?? season.Overview ?? string.Empty;
        
        Id = season.Id;
        Title = title;
        Overview = overview;
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
        string title = episode.Translations
            .FirstOrDefault()?.Title ?? episode.Title ?? string.Empty;
        
        string overview = episode.Translations
            .FirstOrDefault()?.Overview ?? episode.Overview ?? string.Empty;
        
        Id = episode.Id;
        Title = title;
        Overview = overview;
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
}