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

    [JsonProperty("color_palette")]
    public IColorPalettes? ColorPalette { get; set; }

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

    [JsonProperty("duration")] public int? Duration { get; set; }

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

    [JsonProperty("seasons")] public IEnumerable<SeasonDto>? Seasons { get; set; }

    public InfoResponseItemDto(Movie movie)
    {
        Id = movie.Id;
        Title = movie.Title;
        Overview = movie.Overview;
        Type = "movie";
        MediaType = "movie";
        // Watched = tv.Watched;
        // Favorite = tv.Favorite;
        TitleSort = movie.Title.TitleSort(movie.ReleaseDate);
        Duration = movie.Duration;
        Year = movie.ReleaseDate.ParseYear();
        VoteAverage = movie.VoteAverage ?? 0;

        ColorPalette = movie.ColorPalette;
        Backdrop = movie.Backdrop;
        Poster = movie.Poster;
        
        ContentRatings = movie.CertificationMovies
            .Select(certificationMovie => new ContentRating
            {
                Rating = certificationMovie.Certification.Rating,
                Iso31661 = certificationMovie.Certification.Iso31661
            })
            .ToArray() ?? [];
        
        Creators = movie.Crew.Where(crew => crew.Job?.Task == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        Directors = movie.Crew?.Where(crew => crew.Job?.Task == "Director") 
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        Writers = movie.Crew?.Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        // Director = movie.Crew?.Where(crew => crew.Job?.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        //
        Keywords = movie.KeywordMovies
            .Select(keywordMovie => keywordMovie.Keyword.Name)
            .ToArray() ?? [];

        Logo = movie.Media
            .Select(media => media.Type == "logo" ? media.Src : null)
            .FirstOrDefault(logo => logo != null);

        Videos = movie.Media
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray();

        Backdrops = movie.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray() ?? [];

        Posters = movie.Images?
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray() ?? [];

        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray() ?? [];

        Cast = movie.Cast
            .Select(cast => new PeopleDto(cast))
            .ToArray() ?? [];
        
        Crew = movie.Crew?
            .Select(crew => new PeopleDto(crew))
            .ToArray() ?? [];

        Similar = movie.SimilarFrom
            .Select(similar => new RelatedDto(similar, "movie"))
            .ToArray();
        
        Recommendations = movie.RecommendationFrom
            .Select(recommendation => new RelatedDto(recommendation, "movie"))
            .ToArray();
    }

    public InfoResponseItemDto(Tv tv)
    {
        Id = tv.Id;
        Title = tv.Title;
        Overview = tv.Overview;
        Type = tv.Type ?? "tv";
        MediaType = "tv";
        // Watched = tv.Watched;
        // Favorite = tv.Favorite;
        TitleSort = tv.Title.TitleSort(tv.FirstAirDate);
        Duration = tv.Duration;
        NumberOfEpisodes = tv.NumberOfEpisodes;
        HaveEpisodes = tv.HaveEpisodes;
        Year = tv.FirstAirDate.ParseYear();
        VoteAverage = tv.VoteAverage ?? 0;

        ColorPalette = tv.ColorPalette;
        Backdrop = tv.Backdrop;
        Poster = tv.Poster;

        ContentRatings = tv.CertificationTvs
            .Select(certificationTv => new ContentRating
            {
                Rating = certificationTv.Certification.Rating,
                Iso31661 = certificationTv.Certification.Iso31661
            })
            .ToArray() ?? [];

        Creators = tv.Crew?.Where(crew => crew.Job?.Task == "Creator")
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        Directors = tv.Crew?.Where(crew => crew.Job?.Task == "Director") 
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        Writers = tv.Crew?.Where(crew => crew.Job?.Task == "Writer") 
            .Select(crew => new DirectorDto(crew))
            .ToArray() ?? [];
        
        // Director = tv.Crew?.Where(crew => crew.Job?.Task == "Director") 
        //     .Select(crew => new DirectorDto(crew))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = tv.KeywordTvs
            .Select(keywordTv => keywordTv.Keyword.Name)
            .ToArray() ?? [];

        Logo = tv.Media
            .Select(media => media.Type == "logo" ? media.Src : null)
            .FirstOrDefault(logo => logo != null);

        Videos = tv.Media?
            .Where(media => media.Type == "video")
            .Select(media => new VideoDto(media))
            .ToArray() ?? [];

        Backdrops = tv.Images
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .ToArray();

        Posters = tv.Images?
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .ToArray() ?? [];

        Genres = tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        
        Cast = tv.Cast?
            .Select(cast => new PeopleDto(cast))
            .ToArray() ?? [];
        
        Crew = tv.Crew?
            .Select(crew => new PeopleDto(crew))
            .ToArray() ?? [];

        Similar = tv.SimilarFrom
            .Select(similar => new RelatedDto(similar, "tv"))
            .ToArray();
        
        Recommendations = tv.RecommendationFrom
            .Select(recommendation => new RelatedDto(recommendation, "tv"))
            .ToArray();

        Seasons = tv.Seasons
            .OrderBy(season => season.SeasonNumber)?
            .Select(season => new SeasonDto(season));
    }

    public InfoResponseItemDto(Collection collection)
    {
        Id = collection.Id;
        Title = collection.Title;
        Overview = collection.Overview;
        Type = "collections";
        MediaType = "collections";
        // Watched = tv.Watched;
        // Favorite = tv.Favorite;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate
                .ParseYear());
        
        Year = collection.CollectionMovies?
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate
            .ParseYear() ?? 0;
        
        VoteAverage = collection.CollectionMovies?
            .Average(collectionMovie => collectionMovie.Movie.VoteAverage) ?? 0;

        ColorPalette = collection.ColorPalette;
        Backdrop = collection.Backdrop;
        Poster = collection.Poster;

        ContentRatings = collection.CollectionMovies?
            .Select(certificationMovie => new ContentRating
            {
                Rating = certificationMovie.Movie.CertificationMovies.First().Certification.Rating,
                Iso31661 = certificationMovie.Movie.CertificationMovies.First().Certification.Iso31661
            })
            .ToArray() ?? [];

        Creators = collection.CollectionMovies?
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew
               .FirstOrDefault(crew => crew.Job?.Task == "Creator") ?? new Crew()))
            .ToArray() ?? [];
        
        Directors = collection.CollectionMovies?
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew
               .FirstOrDefault(crew => crew.Job?.Task == "Director") ?? new Crew()))
            .ToArray() ?? [];
        
        Writers = collection.CollectionMovies?
            .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew?
               .FirstOrDefault(crew => crew.Job?.Task == "Writer") ?? new Crew()))
            .ToArray() ?? [];
        
        // Director = collection.Movies?
        //     .Select(collectionMovie => new DirectorDto(collectionMovie.Movie.Crew?
        //         .FirstOrDefault(crew => crew.Job?.Task == "Director") ?? new Crew()))
        //     .FirstOrDefault() ?? new DirectorDto(new Crew());
        
        Keywords = collection.CollectionMovies?
            .SelectMany(collectionMovie => collectionMovie.Movie.KeywordMovies)
            .Select(keywordMovie => keywordMovie.Keyword.Name)
            .ToArray() ?? [];

        Logo = collection.CollectionMovies?
            .Select(collectionMovie => collectionMovie.Movie.Media?
               .FirstOrDefault(media => media.Type == "logo")?.Src)
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

public class PeopleDto
{
    [JsonProperty("character", NullValueHandling = NullValueHandling.Ignore)]
    public string? Character { get; set; }

    [JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
    public string? Job { get; set; }

    [JsonProperty("profilePath")] public string? ProfilePath { get; set; }

    [JsonProperty("gender")] public string Gender { get; set; }

    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("knownForDepartment")] public string? KnownForDepartment { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("popularity")] public double Popularity { get; set; }

    [JsonProperty("color_palette", NullValueHandling = NullValueHandling.Include)]
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("deathDay")] public DateTime? DeathDay { get; set; }

    public PeopleDto(Cast cast)
    {
        Character = cast.Role?.Character;
        ProfilePath = cast.Person.Profile;
        KnownForDepartment = cast.Person.KnownForDepartment;
        Name = cast.Person.Name;
        ColorPalette = cast.Person.ColorPalette;
        DeathDay = cast.Person.DeathDay;
        Gender = cast.Person.Gender;
        Id = cast.Person.Id;
    }

    public PeopleDto(Crew crew)
    {
        Job = crew.Job?.Task;
        ProfilePath = crew.Person.Profile;
        KnownForDepartment = crew.Person.KnownForDepartment;
        Name = crew.Person.Name;
        ColorPalette = crew.Person.ColorPalette;
        DeathDay = crew.Person.DeathDay;
        Gender = crew.Person.Gender;
        Id = crew.Person.Id;
    }
}

public class ContentRating
{
    [JsonProperty("rating")] public string Rating { get; set; }

    [JsonProperty("iso31661")] public string Iso31661 { get; set; }
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
        HaveEpisodes = type == "tv" ? similar.TvFrom?.HaveEpisodes : null;
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
        HaveEpisodes = type == "tv" ? recommendation.TvFrom?.HaveEpisodes : null;
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

    [JsonProperty("color_palette")]
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("episodes")] public EpisodeDto[] Episodes { get; set; }

    public SeasonDto(Season season)
    {
        Id = season.Id;
        Overview = season.Overview;
        Poster = season.Poster;
        SeasonNumber = season.SeasonNumber;
        Title = season.Title;
        ColorPalette = season.ColorPalette;
        Episodes = season.Episodes?
            .OrderBy(episode => episode.EpisodeNumber)
            .Select(episode => new EpisodeDto(episode))
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

    [JsonProperty("color_palette")]
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("progress")] public object? Progress { get; set; }

    [JsonProperty("available")] public bool Available { get; set; }

    public EpisodeDto(Episode episode)
    {
        Id = episode.Id;
        EpisodeNumber = episode.EpisodeNumber;
        SeasonNumber = episode.SeasonNumber;
        Title = episode.Title;
        Overview = episode.Overview;
        AirDate = episode.AirDate;
        Still = episode.Still;
        ColorPalette = episode.ColorPalette;
        Available = episode.VideoFiles?.Count > 0;
    }
}