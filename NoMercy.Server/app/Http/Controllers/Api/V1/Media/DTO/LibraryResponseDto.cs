using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public record LibraryResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<LibraryResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, Ulid, string, int, int, Task<Library>> GetLibrary =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id, string language, int take, int page) =>
            mediaContext.Libraries.AsNoTracking()
                .Where(library => library.Id == id)
                .Where(library => library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null
                )
                .Take(take)
                .Skip(page * take)
                .Include(library => library.LibraryMovies
                    .Where(libraryMovie => libraryMovie.Movie.VideoFiles
                        .Any(videoFile => videoFile.Folder != null) == true
                    )
                )
                .ThenInclude(libraryMovie => libraryMovie.Movie)
                .ThenInclude(movie => movie.VideoFiles)
                .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Media)
                .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Images)
                .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
                .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
                .Include(library => library.LibraryTvs
                    .Where(libraryTv => libraryTv.Tv.Episodes
                        .Any(episode => episode.VideoFiles
                            .Any(videoFile => videoFile.Folder != null) == true
                        ) == true
                    )
                )
                .ThenInclude(libraryTv => libraryTv.Tv)
                .ThenInclude(tv => tv.Episodes
                    .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0))
                .ThenInclude(episode => episode.VideoFiles)
                .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Media)
                .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Images)
                .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.GenreTvs)
                .ThenInclude(genreTv => genreTv.Genre)
                .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.CertificationTvs)
                .ThenInclude(certificationTv => certificationTv.Certification)
                .First());
}

public record LibraryResponseItemDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }
    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }

    public LibraryResponseItemDto(LibraryMovie movie)
    {
        Id = movie.Movie.Id.ToString();
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

    public LibraryResponseItemDto(LibraryTv tv)
    {
        Id = tv.Tv.Id.ToString();
        Backdrop = tv.Tv.Backdrop;
        Logo = tv.Tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        Year = tv.Tv.FirstAirDate.ParseYear();
        Overview = tv.Tv.Overview;
        ColorPalette = tv.Tv.ColorPalette;
        Poster = tv.Tv.Poster;
        Title = tv.Tv.Title;
        TitleSort = tv.Tv.Title.TitleSort(tv.Tv.FirstAirDate);

        Type = "tv";
        MediaType = "tv";

        NumberOfItems = tv.Tv.NumberOfEpisodes;
        HaveItems = tv.Tv.Episodes
            .Count(episode => episode.VideoFiles.Any(v => v.Folder != null));

        Genres = tv.Tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        VideoId = tv.Tv.Trailer;
        Videos = tv.Tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(CollectionMovie movie)
    {
        Id = movie.Movie.Id.ToString();
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

    public LibraryResponseItemDto(Collection collection)
    {
        var title = collection.Translations
            .FirstOrDefault()?.Title ?? collection.Title;

        var overview = collection.Translations
            .FirstOrDefault()?.Overview ?? collection.Overview ?? string.Empty;

        Id = collection.Id.ToString();
        Title = title;
        Overview = overview;
        Backdrop = collection.Backdrop;
        Logo = collection.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Year = collection.CollectionMovies
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate.ParseYear();

        ColorPalette = collection.ColorPalette;
        Poster = collection.Poster;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate.ParseYear());

        Type = "specials";
        MediaType = "specials";

        NumberOfItems = collection.Parts;
        HaveItems = collection.CollectionMovies?
            .Count(collectionMovie => collectionMovie.Movie.VideoFiles.Any(v => v.Folder != null));

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

    public LibraryResponseItemDto(Special special)
    {
        var title = special.Title;

        var overview = special.Overview ?? string.Empty;

        Id = special.Id.ToString();
        Title = title;
        Overview = overview;
        Backdrop = special.Backdrop;
        // Logo = special.Images
        //     .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        MediaType = "specials";
        // Year = special.CollectionMovies
        //     .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
        //     ?.Movie.ReleaseDate.ParseYear();

        ColorPalette = special.ColorPalette;
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();

        Type = "specials";
        // NumberOfItems = special.Parts;
        // HaveItems = special.CollectionMovies?
        //     .Where(collectionMovie => collectionMovie.Movie.VideoFiles.Any(v => v.Folder != null))
        //     .Count() ?? 0;
        // Genres = special.CollectionMovies?
        //     .Select(genreTv => genreTv.Movie)
        //     .SelectMany(movie => movie.GenreMovies?
        //         .Select(genreMovie => genreMovie.Genre) ?? [])
        //     .Select(genre => new GenreDto(genre))
        //     .ToArray() ?? [];
        //
        // VideoId = special.CollectionMovies?
        //     .FirstOrDefault()
        //     ?.Movie.Video;
    }
}