using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/userData")]
public class UserDataController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }

    [HttpGet]
    [Route("continue")]
    public async Task<ContinueWatchingDto> ContinueWatching()
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        var continueWatching = await mediaContext.UserData
            .AsNoTracking()
            
            .Where(user => user.UserId == userId)
            
            .Include(userData => userData.Movie)
                .ThenInclude(movie => movie.Media
                    .Where(media => media.Site == "Youtube")
                )
            
            .Include(userData => userData.Tv)
                .ThenInclude(tv => tv.Media
                    .Where(media => media.Site == "Youtube")
                )
            
            .Include(userData => userData.Collection)
                .ThenInclude(collection => collection.CollectionMovies)
                    .ThenInclude(collectionMovie => collectionMovie.Movie)
                        .ThenInclude(movie => movie.Media
                            .Where(media => media.Site == "Youtube")
                        )
            
            .Include(userData => userData.Special)
            .OrderByDescending(userData => userData.UpdatedAt)
            .ToListAsync();

        var filteredContinueWatching = continueWatching
            .DistinctBy(userData => new
            {
                userData.MovieId,
                userData.CollectionId,
                userData.TvId,
                userData.SpecialId
            });

        return new ContinueWatchingDto
        {
            Data = filteredContinueWatching
                .Select(item => new ContinueWatchingItemDto(item))
        };
    }

    [HttpDelete]
    [Route("continue")]
    public async Task<StatusResponseDto<string>> RemoveContinue(UserRequest body)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();

        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.MovieId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.TvId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == Ulid.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "collection" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.CollectionId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            _ => null
        };

        if (userData == null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };
        }

        mediaContext.UserData.Remove(userData);
        await mediaContext.SaveChangesAsync();

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Item removed"
        };
    }

    [HttpGet]
    [Route("watched")]
    public async Task<StatusResponseDto<string>> Watched([FromBody] UserRequest body)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();

        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.MovieId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.TvId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == Ulid.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "collection" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.CollectionId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            _ => null
        };

        if (userData == null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };
        }

        await mediaContext.SaveChangesAsync();

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Item marked as watched"
        };
    }

    [HttpGet]
    [Route("favorites")]
    public async Task<StatusResponseDto<string>> Favorites([FromBody] UserRequest body)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();

        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.MovieId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.TvId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == Ulid.Parse(body.Id))
                .FirstOrDefaultAsync(),
            "collection" => await mediaContext.UserData
                .AsNoTracking()
                .Where(data => data.UserId == userId)
                .Where(data => data.CollectionId == int.Parse(body.Id))
                .FirstOrDefaultAsync(),
            _ => null
        };

        if (userData is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };
        }

        await mediaContext.SaveChangesAsync();

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Item marked as favorite"
        };
    }
}

public class UserRequest
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
}