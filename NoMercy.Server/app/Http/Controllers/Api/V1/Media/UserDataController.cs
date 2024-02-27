using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/userData")]
public class UserDataController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
    
    [HttpGet]
    [Route("continue")]
    public async Task<ContinueWatchingDto> ContinueWatching()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var continueWatching = await mediaContext.UserData
            .AsNoTracking()
            
            .Where(user => user.UserId == userId)
            
            .Include(userData => userData.Movie)
            .Include(userData => userData.Tv)
            .Include(userData => userData.Special)
            
            .OrderByDescending(userData => userData.UpdatedAt)
            .ToListAsync();

        var filteredContinueWatching = continueWatching
            .DistinctBy(userData => new
            {
                userData.MovieId,
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
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();

        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.MovieId == body.Id)
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.TvId == body.Id)
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == body.Id.ToString())
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
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        
        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.MovieId == body.Id)
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.TvId == body.Id)
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == body.Id.ToString())
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
        
        userData.Played = true;

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
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        
        UserData? userData = body.Type switch
        {
            "movie" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.MovieId == body.Id)
                .FirstOrDefaultAsync(),
            "tv" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.TvId == body.Id)
                .FirstOrDefaultAsync(),
            "special" => await mediaContext.UserData.Where(data => data.UserId == userId)
                .Where(data => data.SpecialId == body.Id.ToString())
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
        
        userData.IsFavorite = true;

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
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
}
