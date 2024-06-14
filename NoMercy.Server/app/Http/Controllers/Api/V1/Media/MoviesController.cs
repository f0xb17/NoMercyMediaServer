using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Middleware;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Movies")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/movie/{id:int}")] // match themoviedb.org API
public class MoviesController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Movie(int id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var movie = await InfoResponseDto.GetMovie(mediaContext, userId, id,
            HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault() ?? string.Empty);

        if (movie is not null)
            return Ok(new InfoResponseDto
            {
                Data = new InfoResponseItemDto(movie, HttpContext.Request.Headers.AcceptLanguage[1])
            });

        TmdbMovieClient tmdbMovieClient = new(id);
        var movieAppends = await tmdbMovieClient.WithAllAppends(true);

        if (movieAppends is null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            });

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
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var movie = await InfoResponseDto.GetMovieAvailable(mediaContext, userId, id);

        return new AvailableResponseDto
        {
            Available = movie?.VideoFiles.Any() ?? false
        };
    }

    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var movie = await InfoResponseDto
            .GetMoviePlaylist(mediaContext, userId, id,
                HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault() ?? string.Empty);

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
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .FirstOrDefaultAsync();

        if (movie is null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };

        if (request.Value)
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

            if (movieUser is not null) mediaContext.MovieUser.Remove(movieUser);

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
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };

        foreach (var movie in movies)
            try
            {
                var findMediaFilesJob = new FindMediaFilesJob(movie.Id, movie.Library);
                JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescanning {0} for files",
            Args = new object[]
            {
                movies[0].Title
            }
        };
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<StatusResponseDto<string>> Refresh(int id)
    {
        await using MediaContext mediaContext = new();
        var movies = await mediaContext.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .Include(movie => movie.Library)
            .ToArrayAsync();

        if (movies.Length == 0)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };

        AddMovieJob addMovieJob = new(id);
        JobDispatcher.Dispatch(addMovieJob, "queue", 10);

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Refreshing {0}",
            Args = new object[]
            {
                movies[0].Title
            }
        };
    }
}