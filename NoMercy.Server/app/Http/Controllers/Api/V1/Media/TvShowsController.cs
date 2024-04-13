using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media TV Shows")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/tv/{id:int}")] // match themoviedb.org API
public class TvShowsController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> GetTv(int id)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Tv? tv = await InfoResponseDto.GetTv(mediaContext, userId, id, 
            HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty);

        if (tv is not null && tv.Cast.Count > 0 && tv.Images.Count > 0 && tv.RecommendationFrom.Count > 0)
        {
            return Ok(new InfoResponseDto
            {
                Data = new InfoResponseItemDto(tv, HttpContext.Request.Headers.AcceptLanguage[1])
            });
        }
        
        TvClient tvClient = new(id);
        TvShowAppends? tvShowAppends = await tvClient.WithAllAppends(true);
        
        if (tvShowAppends is null)
        {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            });
        }
        
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
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Tv? tv = await InfoResponseDto.GetTvAvailable(mediaContext, userId, id);

        return new AvailableResponseDto
        {
            Available = tv?.Episodes.Any(episode => episode.VideoFiles.Count > 0) ?? false
        };
    }

    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Tv? tv = await InfoResponseDto
            .GetTvPlaylist(mediaContext, userId, id, HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty);
        
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
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .FirstOrDefaultAsync();

        if (tv is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };
        }

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

            if (tvUser is not null)
            {
                mediaContext.TvUser.Remove(tvUser);
            }

            await mediaContext.SaveChangesAsync();
        }

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                tv.Title ?? "unknown",
                request.Value ? "liked" : "unliked"
            }
        };
    }

    [HttpPost]
    [Route("rescan")]
    public async Task<StatusResponseDto<string>> Like(int id)
    {
        await using MediaContext mediaContext = new();
        var tvs = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .Include(tv => tv.Library)
            .ToArrayAsync();

        if (tvs.Length == 0)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };
        }

        foreach (var tv in tvs)
        {
            try
            {
                FindMediaFilesJob findMediaFilesJob = new FindMediaFilesJob(id: tv.Id, libraryId: tv.Library.Id.ToString());
                JobDispatcher.Dispatch(findMediaFilesJob, "queue", 6);
            }
            catch (Exception e)
            {
                Logger.Encoder(e, Helpers.LogLevel.Error);
            }
        }
        
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescanning {0} for files",
            Args = new object[]
            {
                tvs[0].Title ?? "unknown",
            }
        };
    }
}