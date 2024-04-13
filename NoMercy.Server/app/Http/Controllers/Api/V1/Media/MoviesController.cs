using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using Movie = NoMercy.Database.Models.Movie;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Movies")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/movie/{id:int}")] // match themoviedb.org API
public class MoviesController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> GetMovie(int id)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Movie? movie = await InfoResponseDto.GetMovie(mediaContext, userId, id, 
            HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty);

        if (movie is not null)
        {
            return Ok(new InfoResponseDto
            {
                Data = new InfoResponseItemDto(movie, HttpContext.Request.Headers.AcceptLanguage[1])
            });
        }
        
        MovieClient movieClient = new(id);
        MovieAppends? movieAppends = await movieClient.WithAllAppends(true);
        
        if (movieAppends is null)
        {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            });
        }
        
        AddMovieJob addMovieJob = new(id);
        JobDispatcher.Dispatch(addMovieJob, "queue", 10);
        
        return Ok(new InfoResponseDto
        {
            Data = new InfoResponseItemDto(movieAppends, HttpContext.Request.Headers.AcceptLanguage[1])
        });

    }
    
    [HttpGet]
    [Route("available")]
    public async Task<AvailableResponseDto> Available(int id)
    {        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Movie? movie = await InfoResponseDto.GetMovieAvailable(mediaContext, userId, id);
        
        return new AvailableResponseDto
        {
            Available = movie?.VideoFiles.Any() ?? false
        };
    }
    
    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {        
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        Movie? movie = await InfoResponseDto
            .GetMoviePlaylist(mediaContext, userId, id, HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty);

        return movie is not null
            ?
            [
                new PlaylistResponseDto(movie)
            ]
            : [];
    }

    [HttpPost]
    [Route("like")]
    public async Task<StatusResponseDto<string>> Like(int id, [FromBody] LikeRequestDto request)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            
            .Where(movie => movie.Id == id)
            
            .FirstOrDefaultAsync();

        if (movie is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };
        }
        
        if(request.Value)
        {
            await mediaContext.MovieUser.Upsert(new MovieUser(movie.Id, userId))
                .On(m => new { m.MovieId, m.UserId })
                .WhenMatched(m => new MovieUser
                {
                    MovieId = m.MovieId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var movieUser = await mediaContext.MovieUser
                .Where(movieUser => movieUser.MovieId == movie.Id && movieUser.UserId == userId)
                .FirstOrDefaultAsync();
            
            if(movieUser is not null)
            {
                mediaContext.MovieUser.Remove(movieUser);
            }
            
            await mediaContext.SaveChangesAsync();
        }
        
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0}: {1}",
            Args = new object[]
            {
                movie.Title, 
                request.Value ? "liked" : "unliked"
            }
        };
    }

    [HttpPost]
    [Route("rescan")]
    public async Task<StatusResponseDto<string>> Like(int id)
    {
        await using MediaContext mediaContext = new();
        var movies = await mediaContext.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .Include(movie => movie.Library)
            .ToArrayAsync();

        if (movies.Length == 0)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };
        }

        foreach (var movie in movies)
        {
            try
            {
                FindMediaFilesJob findMediaFilesJob = new FindMediaFilesJob(id: movie.Id, libraryId: movie.Library.Id.ToString());
                JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, Helpers.LogLevel.Error);
            }
        }
        
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescanning {0} for files",
            Args = new object[]
            {
                movies[0].Title,
            }
        };
    }
}
