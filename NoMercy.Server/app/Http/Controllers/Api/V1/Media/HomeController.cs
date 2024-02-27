using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}")]
public class HomeController: Controller
{
    [HttpGet]
    public async Task<HomeResponseDto> Index()
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var genres = await mediaContext.Genres
            .AsNoTracking()
            
            .OrderBy(genre => genre.Name)
            
            .Where(genre => genre.GenreTvShows
                .Any(g => g.Tv.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null))
            
            .Include(genre => genre.GenreMovies
                .Where(genreTv => genreTv.Movie.VideoFiles
                    .Any(videoFile => videoFile.Folder != null) == true
                )
            )
                .ThenInclude(genreMovie => genreMovie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            .Include(genre => genre.GenreTvShows
                .Where(genreTv => genreTv.Tv.Episodes
                    .Any(episode => episode.VideoFiles
                        .Any(videoFile => videoFile.Folder != null)
                    ) == true
                )
            )
                .ThenInclude(genreTv => genreTv.Tv)
                    .ThenInclude(tv => tv.Episodes)
                        .ThenInclude(tv => tv.VideoFiles)
            
            .ToListAsync();
            
        HomeResponseDto result = new HomeResponseDto
        {
            Data = genres.Select(genre => new GenreRowDto
            {
                Title = genre.Name,
                MoreLink = $"/genre/{genre.Id}",
                Id = genre.Id,
                Items = genre.GenreMovies
                    .Select(movie => new GenreRowItemDto(movie))
                    .Concat(genre.GenreTvShows
                        .Select(tv => new GenreRowItemDto(tv)))
                    .Randomize()
                    .Take(36)
            })
        };
        
        return result;
    }
    
    [HttpGet]
    [AllowAnonymous, Route("/status")]
    public StatusResponseDto<string> Status()
    {
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "NoMercy is running!",
            Data = "v1"
        };
    }
    
    [HttpGet]
    [Route("screensaver")]
    public async Task<ScreensaverDto> Screensaver()
    {
        await using MediaContext mediaContext = new();
        var data = await mediaContext.Images
            .AsNoTracking()
            .Where(image =>
                (image.Type == "backdrop" && image.VoteAverage > 2 && image.Iso6391 == null) || 
                (image.Type == "logo" && image.Iso6391 == "en")
            )
            .Include(image => image.Movie)
            .Include(image => image.Tv)
            .ToListAsync();

        var tvCollection = data.Where(image => image is { Type: "backdrop", Tv: not null })
            .DistinctBy(image => image.TvId);
        var movieCollection = data.Where(image => image is { Type: "backdrop", Movie: not null })
            .DistinctBy(image => image.MovieId);
        var logos = data.Where(image => image is { Type: "logo" });
        
        return new ScreensaverDto
        {
            Data = tvCollection
                .Select(image => new ScreensaverDataDto(image, logos, "tv"))
                .Concat(movieCollection.Select(image => new ScreensaverDataDto(image, logos, "movie")))
                .Where(image => image.Meta?.Logo != null)
                .Randomize()
        };
    }
    
    [HttpGet]
    [Route("/api/v{Version:apiVersion}/dashboard/permissions")]
    public async Task<PermissionsResponseDto> UserPermissions()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        User? user = await mediaContext.Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();

        return new PermissionsResponseDto
        {
            Edit = user?.Owner ?? user?.Manage ?? false,
        };
    }
    
}