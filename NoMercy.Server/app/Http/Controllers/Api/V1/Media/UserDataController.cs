using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/userData")]
public class UserDataController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new PlaceholderResponse()
        {
            Data = []
        });
    }

    [HttpGet]
    [Route("continue")]
    public async Task<ContinueWatchingDto> ContinueWatching()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var continueWatching = await mediaContext.UserData
            .AsNoTracking()
            .Where(user => user.UserId == userId)
            .Include(userData => userData.Movie)
            .ThenInclude(movie => movie.Media
                .Where(media => media.Site == "Youtube")
            )
            .Include(userData => userData.Movie)
            .ThenInclude(movie => movie.VideoFiles)
            .Include(userData => userData.Tv)
            .ThenInclude(tv => tv.Media
                .Where(media => media.Site == "Youtube")
            )
            .Include(userData => userData.Tv)
            .ThenInclude(tv => tv.Episodes
                .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0))
            .ThenInclude(episode => episode.VideoFiles)
            .Include(userData => userData.Collection)
            .ThenInclude(collection => collection.CollectionMovies)
            .ThenInclude(collectionMovie => collectionMovie.Movie)
            .ThenInclude(movie => movie.Media
                .Where(media => media.Site == "Youtube")
            )
            .Include(userData => userData.Collection)
            .ThenInclude(collection => collection.CollectionMovies)
            .ThenInclude(collectionMovie => collectionMovie.Movie)
            .ThenInclude(movie => movie.VideoFiles)
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
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();

        var userData = body.Type switch
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
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };

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
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();

        var userData = body.Type switch
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
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };

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
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();

        var userData = body.Type switch
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
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Item not found"
            };

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