using System.Net.Http.Headers;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Models.Search;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media People")]
[ApiVersion("1")]
[Authorize]
public class PeopleController : Controller
{
    [HttpGet]
    [Route("api/v{Version:apiVersion}/person")] // match themoviedb.org API
    public async Task<PeopleResponseDto> Index([FromQuery] PageRequestDto requestDto)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        List<Person> people = await mediaContext.People
            .AsNoTracking()
            .Where(person =>
                person.Casts
                    .Any(cast => cast.Tv.Library.LibraryUsers
                        .FirstOrDefault(u => u.UserId == userId) != null) ||
                person.Casts.Any(cast => cast.Movie.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
            )
            
            .Include(person => person.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .OrderByDescending(person => person.Popularity)
            .Take(requestDto.Take)
            .Skip(requestDto.Page * requestDto.Take)
            .ToListAsync();

        return new PeopleResponseDto
        {
            Data = people.Select(person => new PeopleResponseItemDto(person)),

            NextId = people.Count < requestDto.Take
                ? null
                : people.Count + (requestDto.Page * requestDto.Take)
        };
    }

    [HttpGet]
    [Route("/api/v{Version:apiVersion}/person/{id:int}")] // match themoviedb.org API
    public async Task<PersonResponseDto> Show(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        Person? person = await mediaContext.People
            .AsNoTracking()
            
            // .Where(person =>
            //     person.Casts
            //         .Any(cast => cast.Tv.Library.LibraryUsers
            //             .FirstOrDefault(u => u.UserId == userId) != null) ||
            //     person.Casts.Any(cast => cast.Movie.Library.LibraryUsers
            //         .FirstOrDefault(u => u.UserId == userId) != null)
            // )
            
            .Include(person => person.Casts)
                .ThenInclude(cast => cast.Tv)
                    .ThenInclude(tv => tv.Episodes)
                        .ThenInclude(episode => episode.VideoFiles)
            
            .Include(person => person.Casts)
                .ThenInclude(cast => cast.Role)
            
            .Include(person => person.Casts)
                .ThenInclude(cast => cast.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            .Include(person => person.Casts)
                .ThenInclude(cast => cast.Role)
            
            .Include(person => person.Crews)
                .ThenInclude(crew => crew.Tv)
                    .ThenInclude(tv => tv.Episodes)
                        .ThenInclude(episode => episode.VideoFiles)
            
            .Include(person => person.Crews)
                .ThenInclude(crew => crew.Job)
            
            .Include(person => person.Crews)
                .ThenInclude(crew => crew.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            .Include(person => person.Crews)
                .ThenInclude(crew => crew.Job)
            
            .Include(person => person.Images)
            
            .Include(person => person.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .FirstOrDefaultAsync(person => person.Id == id);

        return new PersonResponseDto
        {
            Data = person is null
                ? null
                : new PersonResponseItemDto(person)
        };
    }
}

public class PageRequestDto
{
    [FromQuery, JsonProperty("page")] public int Page { get; set; }
    [FromQuery, JsonProperty("take")] public int Take { get; set; }
}