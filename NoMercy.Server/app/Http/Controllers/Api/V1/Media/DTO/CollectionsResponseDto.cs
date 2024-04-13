using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class CollectionsResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IEnumerable<CollectionsResponseItemDto> Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<Collection?>> GetCollections =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Collections.AsNoTracking()
                    
                .Where(collection => collection.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
                .Where(collection => collection.CollectionMovies
                    .Any(movie => movie.Movie.VideoFiles.Any()))

                .Include(collection => collection.Library)
                .ThenInclude(library => library.FolderLibraries)
                .ThenInclude(folderLibrary => folderLibrary.Folder)

                .Include(collection => collection.Images)
                .Include(collection => collection.Translations
                    .Where(translation => translation.Iso6391 == language))

                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.VideoFiles)

                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Images)

                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Media)

                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre));

}

public class CollectionsResponseItemDto
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
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }
    [JsonProperty("videoId")] public string? VideoId { get; set; }
    [JsonProperty("videos")] public VideoDto[]? Videos { get; set; } = [];
    
    public CollectionsResponseItemDto(CollectionMovie collectionMovie)
    {
        string? title = collectionMovie.Movie.Translations.FirstOrDefault()?.Title;
        string? overview = collectionMovie.Movie.Translations.FirstOrDefault()?.Overview;

        Id = collectionMovie.Movie.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : collectionMovie.Movie.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : collectionMovie.Movie.Overview;

        Backdrop = collectionMovie.Movie.Backdrop;
        Logo = collectionMovie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "collectionMovie";
        Year = collectionMovie.Movie.ReleaseDate.ParseYear();
        ColorPalette = collectionMovie.Movie.ColorPalette;
        Poster = collectionMovie.Movie.Poster;
        TitleSort = collectionMovie.Movie.Title.TitleSort(collectionMovie.Movie.ReleaseDate);
        Type = "collectionMovie";
        Genres = collectionMovie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = collectionMovie.Movie.Video;
        Videos = collectionMovie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public CollectionsResponseItemDto(Collection collection)
    {
        string? title = collection.Translations.FirstOrDefault()?.Title;
        string? overview = collection.Translations.FirstOrDefault()?.Overview;

        Id = collection.Id;
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : collection.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : collection.Overview;
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
}
