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
[Tags("Media TV Shows")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/tv/{id:int}")] // match themoviedb.org API
public class TvShowsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Tv(int id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var tv = await InfoResponseDto.GetTv(mediaContext, userId, id,
            HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault() ?? string.Empty);

        if (tv is not null)
            return Ok(new InfoResponseDto
            {
                Data = new InfoResponseItemDto(tv, HttpContext.Request.Headers.AcceptLanguage[1])
            });

        TmdbTvClient tmdbTvClient = new(id);
        var tvShowAppends = await tmdbTvClient.WithAllAppends(true);

        if (tvShowAppends is null)
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            });

        AddShowJob addShowJob = new(id);
        JobDispatcher.Dispatch(addShowJob, "queue", 10);

        return Ok(new InfoResponseDto
        {
            Data = new InfoResponseItemDto(tvShowAppends, HttpContext.Request.Headers.AcceptLanguage[1])
        });
    }

    [HttpGet]
    [Route("available")]
    public async Task<AvailableResponseDto> Available(int id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var tv = await InfoResponseDto.GetTvAvailable(mediaContext, userId, id);

        return new AvailableResponseDto
        {
            Available = tv?.Episodes.Any(episode => episode.VideoFiles.Count > 0) ?? false
        };
    }

    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var tv = await InfoResponseDto
            .GetTvPlaylist(mediaContext, userId, id,
                HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault() ?? string.Empty);

        var episodes = tv?.Seasons
            .Where(season => season.SeasonNumber > 0)
            .SelectMany(season => season.Episodes)
            .Select(episode => new PlaylistResponseDto(episode))
            .ToArray() ?? [];

        var extras = tv?.Seasons
            .Where(season => season.SeasonNumber == 0)
            .SelectMany(season => season.Episodes)
            .Select(episode => new PlaylistResponseDto(episode))
            .ToArray() ?? [];

        return episodes.Concat(extras).ToArray();
    }

    [HttpPost]
    [Route("like")]
    public async Task<StatusResponseDto<string>> Like(int id, [FromBody] LikeRequestDto request)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .FirstOrDefaultAsync();

        if (tv is null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };

        if (request.Value)
        {
            await mediaContext.TvUser.Upsert(new TvUser(tv.Id, userId))
                .On(m => new { m.TvId, m.UserId })
                .WhenMatched(m => new TvUser
                {
                    TvId = m.TvId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var tvUser = await mediaContext.TvUser
                .Where(tvUser => tvUser.TvId == tv.Id && tvUser.UserId == userId)
                .FirstOrDefaultAsync();

            if (tvUser is not null) mediaContext.TvUser.Remove(tvUser);

            await mediaContext.SaveChangesAsync();
        }

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                tv.Title,
                request.Value ? "liked" : "unliked"
            }
        };
    }

    [HttpPost]
    [Route("rescan")]
    public async Task<StatusResponseDto<string>> Rescan(int id)
    {
        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .Include(tv => tv.Library)
            .FirstOrDefaultAsync();

        if (tv is null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };

        try
        {
            FindMediaFilesJob findMediaFilesJob = new(tv.Id, tv.Library);
            JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);
        }
        catch (Exception e)
        {
            Logger.Encoder(e, LogLevel.Error);
        }

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescanning {0} for files",
            Args = new object[]
            {
                tv.Title
            }
        };
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<StatusResponseDto<string>> Refresh(int id)
    {
        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .Include(tv => tv.Library)
            .FirstOrDefaultAsync();

        if (tv is null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };

        AddShowJob addShowJob = new(id);
        JobDispatcher.Dispatch(addShowJob, "queue", 10);

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescanning {0} for files",
            Args = new object[]
            {
                tv.Title
            }
        };
    }
}