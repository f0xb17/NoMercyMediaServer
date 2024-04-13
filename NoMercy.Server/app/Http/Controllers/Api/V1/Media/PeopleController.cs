using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using Person = NoMercy.Database.Models.Person;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media People")]
[ApiVersion("1")]
[Authorize]
public class PeopleController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    [Route("api/v{Version:apiVersion}/person")] // match themoviedb.org API
    public async Task<PeopleResponseDto> Index([FromQuery] PageRequestDto requestDto)
    {
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        List<Person> people = await mediaContext.People
            .AsNoTracking()
            
            .Where(person =>
                person.Casts
                    .Any(cast => cast.Tv != null && cast.Tv.Library.LibraryUsers
                        .FirstOrDefault(u => u.UserId == userId) != null) ||
                person.Casts.Any(cast => cast.Movie != null && cast.Movie.Library.LibraryUsers
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
        PersonClient personClient = new(id);
        PersonAppends? personAppends = await personClient.WithAllAppends(true);
        
        if (personAppends is null)
        {
            return new PersonResponseDto
            {
                Data = null
            };
        }
        
        return new PersonResponseDto
        {
            Data = new PersonResponseItemDto(personAppends, HttpContext.Request.Headers.AcceptLanguage[1])
        };
    }
}

public class PageRequestDto
{
    [FromQuery, JsonProperty("page")] public int Page { get; set; } = 1;
    [FromQuery, JsonProperty("take")] public int Take { get; set; } = 500;
    [FromQuery, JsonProperty("version")] public string? Version { get; set; }
}