using System.Net.Http.Headers;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Movies")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/movie/{id:int}")]
public class MoviesController : Controller
{
    [HttpGet]
    public async Task<InfoResponseDto> Movie(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Include(movie => movie.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(movie => movie.Media)
            .Include(movie => movie.AlternativeTitles)
            
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == "en" || translation.Iso6391 == "nl"))
            
            .Include(movie => movie.Images
                .Where(image => 
                    (image.Type == "logo" && (image.Iso6391 == "en" || image.Iso6391 == null))
                    || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                ))
            
            .Include(movie => movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
            
            .Include(movie => movie.KeywordMovies)
                .ThenInclude(keywordMovie => keywordMovie.Keyword)
            
            .Include(movie => movie.Cast)
                .ThenInclude(castMovie => castMovie.Person)
            
            .Include(movie => movie.Cast)
                .ThenInclude(castMovie => castMovie.Role)
            
            .Include(movie => movie.Crew)
                .ThenInclude(crewMovie => crewMovie.Person)
            
            .Include(movie => movie.Crew)
                .ThenInclude(crewMovie => crewMovie.Job)
            
            .Include(movie => movie.RecommendationFrom)
            
            .Include(movie => movie.SimilarFrom)
            
            .FirstOrDefaultAsync();

        return new InfoResponseDto
        {
            Data = movie is not null
                ? new InfoResponseItemDto(movie) 
                : null
        };
    }
    
    [HttpGet]
    [Route("available")]
    public async Task<AvailableResponseDto> Available(int id)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            
            .Where(movie => movie.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Where(movie => movie.Id == id)
                .Include(movie => movie.VideoFiles)
            .FirstOrDefaultAsync();
        
        return new AvailableResponseDto
        {
            Available = movie?.VideoFiles.Any() ?? false
        };
    }
    
    [HttpGet]
    [Route("watch")]
    public Task<object> Watch(int id)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        using HttpClient client = new();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", (string)HttpContext.Request.Headers["Authorization"]!);
        
        return Task.FromResult<object>(client.GetAsync(
                $"https://192-168-2-201.1968dcdc-bde6-4a0f-a7b8-5af17afd8fb6.nomercy.tv:7635/api/movie/{id}/watch")
            .Result.Content.ReadAsStringAsync().Result);
    }
}