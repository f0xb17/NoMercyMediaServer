using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class SpecialsResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IEnumerable<SpecialsResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<Special?>> GetSpecials =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Specials.AsNoTracking()
                .Include(special => special.Items)
                    .ThenInclude(item => item.Episode)
                        .ThenInclude(episode => episode.Tv)
            
                .Include(special => special.Items)
                    .ThenInclude(item => item.Movie)
            );


    // .Where(special => special.Library.LibraryUsers
    //     .FirstOrDefault(u => u.UserId == userId) != null)
    // .Where(special => special.SpecialMovies
    //     .Any(movie => movie.Movie.VideoFiles.Any()))
    //
    // .Include(special => special.Library)
    // .ThenInclude(library => library.FolderLibraries)
    // .ThenInclude(folderLibrary => folderLibrary.Folder)
    //
    // .Include(special => special.Images)
    // .Include(special => special.Translations
    //     .Where(translation => translation.Iso6391 == language))
    //
    // .Include(special => special.SpecialMovies)
    // .ThenInclude(movie => movie.Movie)
    // .ThenInclude(movie => movie.VideoFiles)
    //
    // .Include(special => special.SpecialMovies)
    // .ThenInclude(movie => movie.Movie)
    // .ThenInclude(movie => movie.Images)
    //
    // .Include(special => special.SpecialMovies)
    // .ThenInclude(movie => movie.Movie)
    // .ThenInclude(movie => movie.Media)
    //
    // .Include(special => special.SpecialMovies)
    // .ThenInclude(movie => movie.Movie)
    // .ThenInclude(movie => movie.GenreMovies)
    // .ThenInclude(genreMovie => genreMovie.Genre));

}

public class SpecialsResponseItemDto
{
    [JsonProperty("id")] public string Id { get; set; }
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
    
    public SpecialsResponseItemDto(SpecialItem item)
    {
        if (item.Movie is null) return;
        
        string? title = item.Movie.Translations.FirstOrDefault()?.Title;
        string? overview = item.Movie.Translations.FirstOrDefault()?.Overview;

        Id = item.Movie.Id.ToString();
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : item.Movie.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : item.Movie.Overview;

        Backdrop = item.Movie.Backdrop;
        Logo = item.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "item";
        Year = item.Movie.ReleaseDate.ParseYear();
        ColorPalette = item.Movie.ColorPalette;
        Poster = item.Movie.Poster;
        TitleSort = item.Movie.Title.TitleSort(item.Movie.ReleaseDate);
        Type = "item";
        Genres = item.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = item.Movie.Video;
        Videos = item.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public SpecialsResponseItemDto(Special special)
    {
        Id = special.Id.ToString();
        Title = special.Title;
        Overview = special.Description;
        Backdrop = special.Backdrop;
        // Logo = special.Images
        //     .FirstOrDefault(media => media.Type == "logo")?.FilePath;
        
        MediaType = "specials";
        Year = special.CreatedAt.ParseYear();
        
        ColorPalette = special.ColorPalette;
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();
        
        Type = "specials";
        
        NumberOfEpisodes = special.Items
            .Select(item => item.Episode?.Tv?.NumberOfEpisodes).Count() + special.Items.Count(item => item.MovieId != null);

        HaveEpisodes = special.Items.Count;

        // VideoId = special.SpecialMovies?
        //     .FirstOrDefault()
        //     ?.Movie.Video;
    }
}
