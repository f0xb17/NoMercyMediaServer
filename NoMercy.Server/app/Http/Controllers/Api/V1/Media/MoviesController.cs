using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Movies")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/movie/{id:int}")] // match themoviedb.org API
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
            
            .Include(movie => movie.MovieUser)
            
            .Include(movie => movie.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(movie => movie.Media)
            .Include(movie => movie.AlternativeTitles)
            
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(movie => movie.Images
                .Where(image => 
                    (image.Type == "logo" && image.Iso6391 == "en")
                    || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                ))
            
            .Include(movie => movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
            
            .Include(movie => movie.KeywordMovies)
                .ThenInclude(keywordMovie => keywordMovie.Keyword)
            
            .Include(movie => movie.Cast.Where(cast => cast.Role != null && cast.Role.Character != null))
                .ThenInclude(castMovie => castMovie.Person)
            
            .Include(movie => movie.Cast.Where(cast => cast.Role != null && cast.Role.Character != null))
                .ThenInclude(castMovie => castMovie.Role)
            
            .Include(movie => movie.Crew.Where(cast => cast.Job != null && cast.Job.Task != null))
                .ThenInclude(crewMovie => crewMovie.Person)
            
            .Include(movie => movie.Crew.Where(cast => cast.Job != null && cast.Job.Task != null))
                .ThenInclude(crewMovie => crewMovie.Job)
            
            .Include(movie => movie.RecommendationFrom)
            
            .Include(movie => movie.SimilarFrom)
            
            .Include(movie => movie.VideoFiles)
                .ThenInclude(file => file.UserData)
            
            .FirstOrDefaultAsync();

        return new InfoResponseDto
        {
            Data = movie is not null
                ? new InfoResponseItemDto(movie, HttpContext.Request.Headers.AcceptLanguage[1] ?? "US") 
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
            Available = movie?.VideoFiles.Count != 0
        };
    }
    
    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            
            .Where(movie => movie.Id == id)
            
            .Where(movie => movie.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            
            .Include(movie => movie.Media
                .Where(media => media.Type == "video"))
            
            .Include(movie => movie.Images
                .Where(image => image.Type == "logo"))
            
            .Include(movie => movie.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(movie => movie.VideoFiles)
                .ThenInclude(file => file.UserData)
            
            .FirstOrDefaultAsync();

        return movie is not null
            ?
            [
                new PlaylistResponseDto(movie)
            ]
            : [];
    }

    [HttpPost]
    [Route("like")]
    public async Task<StatusResponseDto<string>> Like(int id, [FromBody] LikeRequestDto request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        var movie = await mediaContext.Movies
            .AsNoTracking()
            
            .Where(movie => movie.Id == id)
            
            .FirstOrDefaultAsync();

        if (movie is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Movie not found"
            };
        }
        
        if(request.Like)
        {
            await mediaContext.MovieUser.Upsert(new MovieUser(movie.Id, userId))
                .On(m => new { m.MovieId, m.UserId })
                .WhenMatched(m => new MovieUser
                {
                    MovieId = m.MovieId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var movieUser = await mediaContext.MovieUser
                .Where(movieUser => movieUser.MovieId == movie.Id && movieUser.UserId == userId)
                .FirstOrDefaultAsync();
            
            if(movieUser is not null)
            {
                mediaContext.MovieUser.Remove(movieUser);
            }
            
            await mediaContext.SaveChangesAsync();
        }
        
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0}: {1}",
            Args = new object[]
            {
                movie.Title, 
                request.Like ? "liked" : "unliked"
            }
        };
    }
}
