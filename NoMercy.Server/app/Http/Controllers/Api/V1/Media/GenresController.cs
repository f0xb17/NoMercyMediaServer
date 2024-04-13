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
using static NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO.GenresResponseDto;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Genres")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/genres")]
public class GenresController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<GenresResponseDto> GetGenres()
    {       
        List<GenresResponseItemDto> genres = [];
        Guid userId = GetUserId();
    
        await using MediaContext mediaContext = new();
        await foreach (Genre genre in GenresResponseDto.GetGenres(mediaContext, userId, 
                           HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty))
        {
            genres.Add(new GenresResponseItemDto(genre));
        }
    
        return new GenresResponseDto
        {
            Data = genres.OrderBy(genre => genre.Title)
        };
    }
    
    [HttpGet]
    [Route("{genreId}")]
    public async Task<IActionResult> GetGenre(int genreId, [FromQuery] PageRequestDto request)
    {        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        var library = await GenreResponseDto.GetGenre(mediaContext, userId, genreId, 
            HttpContext.Request.Headers.AcceptLanguage[0] ?? string.Empty, request.Take, request.Page);

        if (request.Version != "lolomo")
        {
            return Ok(new GenreResponseDto
            {
                Data = library.GenreMovies
                    .Select(movie => new GenreResponseItemDto(movie))
                    .Concat(library.GenreTvShows
                        .Select(tv => new GenreResponseItemDto(tv)))
                    
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort),

                NextId = (library.GenreMovies.Count + library.GenreTvShows.Count) < request.Take
                    ? null
                    : (library.GenreMovies.Count + library.GenreTvShows.Count) + (request.Page * request.Take)
            });
        }
        
        string[] numbers = ["*", "#", "'", "\"", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];
        string[] letters = ["#", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

        return Ok(new LoloMoResponseDto<GenreResponseItemDto>
        {
            Data = letters.Select(genre => new LoloMoRowDto<GenreResponseItemDto>
            {
                Title = genre,
                Id = genre,
            
                Items = library.GenreMovies
                    .Where(libraryMovie => genre == "#" 
                        ? numbers.Any(p=> libraryMovie.Movie.Title.StartsWith(p))
                        : libraryMovie.Movie.Title.StartsWith(genre))
                    .Select(movie => new GenreResponseItemDto(movie))
                    
                    .Concat(library.GenreTvShows
                        .Where(libraryTv => genre == "#" 
                            ? numbers.Any(p=> libraryTv.Tv.Title.StartsWith(p))
                            : libraryTv.Tv.Title.StartsWith(genre))
                        .Select(tv => new GenreResponseItemDto(tv)))
                    
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort)
            })
        });
    }

}