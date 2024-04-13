using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Libraries")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/libraries")]
public class LibrariesController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<LibrariesResponseDto> GetLibraries()
    {       
        List<LibrariesResponseItemDto> libraries = [];
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (Library? library in LibrariesResponseDto.GetLibraries(mediaContext, userId, 
                           HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty))
        {
            if (library is null) continue;
            libraries.Add(new LibrariesResponseItemDto(library));
        }

        return new LibrariesResponseDto
        {
            Data = libraries.OrderBy(library => library.Order)
        };
    }
    
    [HttpGet]
    [Route("{libraryId}")]
    public async Task<IActionResult> GetLibrary(Ulid libraryId, [FromQuery] PageRequestDto request)
    {        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        var library = await LibraryResponseDto.GetLibrary(mediaContext, userId, libraryId, 
            HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty, request.Take, request.Page);

        if (request.Version != "lolomo")
        {
            return Ok(new LibraryResponseDto
            {
                Data = library.LibraryMovies
                    .Select(movie => new LibraryResponseItemDto(movie))
                    .Concat(library.LibraryTvs
                        .Select(tv => new LibraryResponseItemDto(tv)))
                    
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort),

                NextId = (library.LibraryMovies.Count + library.LibraryTvs.Count) < request.Take
                    ? null
                    : (library.LibraryMovies.Count + library.LibraryTvs.Count) + (request.Page * request.Take)
            });
        }
        
        string[] numbers = ["*", "#", "'", "\"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
        string[] letters = ["#", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

        return Ok(new LoloMoResponseDto<LibraryResponseItemDto>
        {
            Data = letters.Select(genre => new LoloMoRowDto<LibraryResponseItemDto>
            {
                Title = genre,
                Id = genre,
            
                Items = library.LibraryMovies
                    .Where(libraryMovie => genre == "#" 
                        ? numbers.Any(p=> libraryMovie.Movie.Title.StartsWith(p))
                        : libraryMovie.Movie.Title.StartsWith(genre))
                    .Select(movie => new LibraryResponseItemDto(movie))
                    
                    .Concat(library.LibraryTvs
                        .Where(libraryTv => genre == "#" 
                            ? numbers.Any(p=> libraryTv.Tv.Title.StartsWith(p))
                            : libraryTv.Tv.Title.StartsWith(genre))
                        .Select(tv => new LibraryResponseItemDto(tv)))
                    
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort)
            })
        });
    }

}