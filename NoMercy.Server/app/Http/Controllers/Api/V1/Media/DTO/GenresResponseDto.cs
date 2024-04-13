using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class GenresResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<GenresResponseItemDto> Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<Genre>> GetGenres =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Genres.AsNoTracking()
                
                .Include(genre => genre.GenreMovies
                    .Where(genreMovie => genreMovie.Movie.MovieUser
                        .FirstOrDefault(u => u.UserId == userId) != null
                    )

                    .Where(genreMovie => genreMovie.Movie.VideoFiles
                        .Any(videoFile => videoFile.Folder != null) == true
                    )
                )
                    .ThenInclude(genreMovie => genreMovie.Movie)
                
                .Include(genre => genre.GenreMovies)
                    .ThenInclude(genreMovie => genreMovie.Movie.Translations
                        
                    .Where(translation => translation.Iso6391 == language))
                    .Include(genre => genre.GenreMovies)
                        .ThenInclude(genreMovie => genreMovie.Movie.CertificationMovies)
                            .ThenInclude(certificationMovie => certificationMovie.Certification)
                
                .Include(genre => genre.GenreTvShows
                    .Where(genreMovie => genreMovie.Tv.TvUser
                        .FirstOrDefault(u => u.UserId == userId) != null
                    )
                    .Where(genreTv => genreTv.Tv.Episodes
                        .Any(episode => episode.VideoFiles
                            .Any(videoFile => videoFile.Folder != null) == true
                        ) == true
                    )
                )
                    .ThenInclude(genreTv => genreTv.Tv)
                        .ThenInclude(tv => tv.Episodes
                        .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0))
                
                .Include(genre => genre.GenreTvShows)
                    .ThenInclude(genreTv => genreTv.Tv.Media)
                
                .Include(genre => genre.GenreTvShows)
                    .ThenInclude(genreTv => genreTv.Tv.Translations
                    .Where(translation => translation.Iso6391 == language))
                
                .Include(genre => genre.GenreTvShows)
                    .ThenInclude(genreTv => genreTv.Tv.CertificationTvs)
                        .ThenInclude(certificationTv => certificationTv.Certification));
}

public class GenresResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }

    [JsonProperty("favorite")] public bool Favorite { get; set; }

    [JsonProperty("watched")] public bool Watched { get; set; }

    [JsonProperty("logo")] public string? Logo { get; set; }

    [JsonProperty("mediaType")] public string MediaType { get; set; }

    [JsonProperty("number_of_episodes")] public int? NumberOfEpisodes { get; set; }

    [JsonProperty("have_episodes")] public int? HaveEpisodes { get; set; }

    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("color_palette")] 
    public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("year")] public int? Year { get; set; }

    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }
    
    public GenresResponseItemDto(GenreMovie movie)
    {
        Id = movie.Movie.Id;
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.Movie.ReleaseDate.ParseYear();
        Overview = movie.Movie.Overview;
        ColorPalette = movie.Movie.ColorPalette;
        Poster = movie.Movie.Poster;
        Title = movie.Movie.Title;
        TitleSort = movie.Movie.Title
            .TitleSort(movie.Movie.ReleaseDate);
        Type = "movie";
        
        Genres = movie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public GenresResponseItemDto(GenreTv tv)
    {
        Id = tv.Tv.Id;
        Backdrop = tv.Tv.Backdrop;
        Logo = tv.Tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "tv";
        Year = tv.Tv.FirstAirDate.ParseYear();
        Overview = tv.Tv.Overview;
        ColorPalette = tv.Tv.ColorPalette;
        Poster = tv.Tv.Poster;
        Title = tv.Tv.Title;
        TitleSort = tv.Tv.Title.TitleSort(tv.Tv.FirstAirDate);
        Type = "tv";
        NumberOfEpisodes = tv.Tv.NumberOfEpisodes;
        
        HaveEpisodes = tv.Tv.Episodes.Count;
            
        Genres = tv.Tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        VideoId = tv.Tv.Trailer;
        Videos = tv.Tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public GenresResponseItemDto(CollectionMovie movie)
    {
        Id = movie.Movie.Id;
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.Movie.ReleaseDate.ParseYear();
        Overview = movie.Movie.Overview;
        ColorPalette = movie.Movie.ColorPalette;
        Poster = movie.Movie.Poster;
        Title = movie.Movie.Title;
        TitleSort = movie.Movie.Title
            .TitleSort(movie.Movie.ReleaseDate);
        Type = "movie";
        
        Genres = movie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public GenresResponseItemDto(Collection collection)
    {
        string title = collection.Translations
            .FirstOrDefault()?.Title ?? collection.Title;
        
        string overview = collection.Translations
            .FirstOrDefault()?.Overview ?? collection.Overview ?? string.Empty;

        Id = collection.Id;
        Title = title;
        Overview = overview;
        Backdrop = collection.Backdrop;
        Logo = collection.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;
        
        MediaType = "collection";
        Year = collection.CollectionMovies
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate.ParseYear();
        
        ColorPalette = collection.ColorPalette;
        Poster = collection.Poster;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate.ParseYear());
        
        Type = "collection";
        NumberOfEpisodes = collection.Parts;
        HaveEpisodes = collection.CollectionMovies?
            .Where(collectionMovie => collectionMovie.Movie.VideoFiles.Any(v => v.Folder != null))
            .Count() ?? 0;
        Genres = collection.CollectionMovies?
            .Select(genreTv => genreTv.Movie)
            .SelectMany(movie => movie.GenreMovies?
                .Select(genreMovie => genreMovie.Genre) ?? [])
            .Select(genre => new GenreDto(genre))
            .ToArray() ?? [];
        
        VideoId = collection.CollectionMovies?
            .FirstOrDefault()
            ?.Movie.Video;
    }

    public GenresResponseItemDto(Genre genre)
    {
        Id = genre.Id;
        Title = genre.Name;
        
        MediaType = "genres";
        Year = genre.GenreMovies
            .MinBy(genreMovie => genreMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate.ParseYear();
        
        TitleSort = genre.Name
            .TitleSort(genre.GenreMovies
                .MinBy(genreMovie => genreMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate.ParseYear());
        
        Type = "genres";
        NumberOfEpisodes = genre.GenreMovies.Count;
        HaveEpisodes = genre.GenreMovies
            .Count(genreMovie => genreMovie.Movie.VideoFiles.Any(v => v.Folder != null));
        Genres = genre.GenreMovies
            .Select(genreMovie => genreMovie.Movie)
            .SelectMany(movie => movie.GenreMovies
                .Select(genreMovie => genreMovie.Genre))
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
    }
}
