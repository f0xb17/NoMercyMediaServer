using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Specials")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/specials")]
public class SpecialController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PageRequestDto request)
    {
        List<Special> specials = [];
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (Special? special in SpecialsResponseDto.GetSpecials(mediaContext, userId, 
                           HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty))
        {
            if (special is null) continue;
            specials.Add(special);
        }
        
        if (request.Version != "lolomo")
        {
            return Ok(new SpecialsResponseDto
            {
                Data = specials.Select(special => new SpecialsResponseItemDto(special))
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort)
            });
        }
        
        string[] numbers = ["*", "#", "'", "\"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
        string[] letters = ["#", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", 
            "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

        return Ok(new LoloMoResponseDto<LibraryResponseItemDto>
        {
            Data = letters.Select(genre => new LoloMoRowDto<LibraryResponseItemDto>
            {
                Title = genre,
                Id = genre,
            
                Items = specials.Where(special => genre == "#" 
                        ? numbers.Any(p=> special.Title.StartsWith(p))
                        : special.Title.StartsWith(genre))
                    .Select(special => new LibraryResponseItemDto(special))
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort)
            })
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Show(Ulid id)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        Special? special = await SpecialResponseDto.GetSpecial(mediaContext, userId, id, 
            HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty, 
            HttpContext.Request.Headers.AcceptLanguage[1] ?? string.Empty);
        
        IEnumerable<int> movieIds = special?.Items
            .Where(item => item.MovieId is not null)
            .Select(item => item.MovieId ?? 0) ?? [];
        
        IEnumerable<int> episodeIds = special?.Items
            .Where(item => item.EpisodeId is not null)
            .Select(item => item.EpisodeId ?? 0) ?? [];
        
        List<SpecialItemsDto> items = [];
        await foreach (Movie? movie in SpecialResponseDto.GetSpecialMovies(mediaContext, userId, movieIds,
                           HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty,
                           HttpContext.Request.Headers.AcceptLanguage[1] ?? string.Empty))
        {
            items.Add(new SpecialItemsDto(movie));
        }
        
        await foreach (Tv? tv in SpecialResponseDto.GetSpecialTvs(mediaContext, userId, episodeIds,
                           HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty,
                           HttpContext.Request.Headers.AcceptLanguage[1] ?? string.Empty))
        {
            items.Add(new SpecialItemsDto(tv));
        }

        if (special is not null && special.Items.Count > 0)
        {
            return Ok(new SpecialResponseDto
            {
                Data = new SpecialResponseItemDto(special, items)
            });
        }
        
        return NotFound(new SpecialResponseDto
        {
            Data = null
        });
    }

    [HttpGet]
    [Route("{id}/available")]
    public async Task<IActionResult> Available(Ulid id)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        var special = await SpecialResponseDto
            .GetSpecialAvailable(mediaContext, userId, id);
        
        return Ok(new AvailableResponseDto
        {
            Available = special is not null 
                && (
                    special.Items
                        .Select(movie => movie.Movie?.VideoFiles)
                        .Any() 
                    
                    || special.Items
                        .Select(movie => movie.Episode?.VideoFiles)
                        .Any()
                )
        });
    }

    [HttpGet]
    [Route("{id}/watch")]
    public async Task<IActionResult> Watch(Ulid id)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();

        var special = await SpecialResponseDto
            .GetSpecialPlaylist(mediaContext, userId, id, HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty);
        
        var items = special?.Items
            .OrderBy(item => item.Order)
            .Select((item, index) => item.EpisodeId is not null
                ? new PlaylistResponseDto(item.Episode ?? new Episode(), index)
                : new PlaylistResponseDto(item.Movie ?? new Movie(), index)
            )
            .ToArray();

        return Ok(items);
    }
    [HttpPost]
    
    [Route("{id}/like")]
    public async Task<IActionResult> Like(Ulid id, [FromBody] LikeRequestDto request)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Specials
            .AsNoTracking()
            .Where(collection => collection.Id == id)
            
            .FirstOrDefaultAsync();

        if (collection is null)
        {
            return Ok(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Special not found"
            });
        }
        
        if(request.Value)
        {
            await mediaContext.SpecialUser.Upsert(new SpecialUser(collection.Id, userId))
                .On(m => new { m.SpecialId, m.UserId })
                .WhenMatched(m => new SpecialUser
                {
                    SpecialId = m.SpecialId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var collectionUser = await mediaContext.SpecialUser
                .Where(collectionUser => collectionUser.SpecialId == collection.Id && collectionUser.UserId == userId)
                .FirstOrDefaultAsync();
            
            if(collectionUser is not null)
            {
                mediaContext.SpecialUser.Remove(collectionUser);
            }
            
            await mediaContext.SaveChangesAsync();
        }
        
        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                collection.Title,
                request.Value ? "liked" : "unliked"
            }
        });
    }
}