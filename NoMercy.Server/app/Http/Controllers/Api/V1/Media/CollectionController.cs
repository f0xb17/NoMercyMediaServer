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
[Tags("Media Collections")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/collection")] // match themoviedb.org API
public class CollectionsController : Controller
{
    [HttpGet]
    public async Task<CollectionsResponseDto> Collections()
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var libraries = await mediaContext.Collections
            .AsNoTracking()
            
            .Where(collection => collection.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Where(collection => collection.CollectionMovies
                .Any(movie => movie.Movie.VideoFiles.Any()))
            
            .Include(collection => collection.Library)
                .ThenInclude(library => library.FolderLibraries)
                    .ThenInclude(folderLibrary => folderLibrary.Folder)
            
            .Include(collection => collection.Images)
            .Include(collection => collection.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            .OrderBy(collection => collection.TitleSort)
            
            .ToListAsync();

        return new CollectionsResponseDto
        {
            Data = libraries.Select(library => new CollectionsResponseItemDto(library))
        };
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<CollectionResponseDto> Collection(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Collections
            .AsNoTracking()
            
            .Where(collection => collection.Id == id)
            
            .Where(collection => collection.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Include(collection => collection.CollectionUser)
            
            .Include(collection => collection.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.Translations
                        .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.MovieUser)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.CertificationMovies)
                        .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.GenreMovies)
                        .ThenInclude(genreMovie => genreMovie.Genre)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.Cast.Where(cast => cast.Role.Character != null))
                        .ThenInclude(genreMovie => genreMovie.Person)
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.Cast.Where(cast => cast.Role.Character != null))
                        .ThenInclude(genreMovie => genreMovie.Role)
            
            // .Include(collection => collection.CollectionMovies)
            //     .ThenInclude(movie => movie.Movie)
            //         .ThenInclude(movie => movie.Crew.Where(cast => cast.Job != null && cast.Job.Task != null))
            //             .ThenInclude(genreMovie => genreMovie.Person)
            //
            // .Include(collection => collection.CollectionMovies)
            //     .ThenInclude(movie => movie.Movie)
            //         .ThenInclude(movie => movie.Crew.Where(cast => cast.Job != null && cast.Job.Task != null))
            //             .ThenInclude(genreMovie => genreMovie.Job)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.Images)
            
            .Include(collection => collection.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(collection => collection.Images
                .Where(image => 
                    (image.Type == "logo" && image.Iso6391 == "en")
                    || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                )
            )

            .FirstOrDefaultAsync();
        
        return new CollectionResponseDto
        {
            Data = collection is not null 
                ? new CollectionResponseItemDto(collection, HttpContext.Request.Headers.AcceptLanguage[1] ?? "US")
                : null
        };
    }
    
    [HttpGet]
    [Route("{id:int}/available")]
    public async Task<AvailableResponseDto> Available(int id)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Collections
            .AsNoTracking()
            
            .Where(collection => collection.Id == id)
            
            .Where(collection => collection.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Include(movie => movie.CollectionMovies)
                .Where(collectionMovie => collectionMovie.CollectionMovies
                    .Any(movie => movie.Movie.VideoFiles.Any()))
            
            .Include(movie => movie.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
                        .ThenInclude(file => file.UserData)
            
            .FirstOrDefaultAsync();
        
        return new AvailableResponseDto
        {
            Available = collection is not null && collection.CollectionMovies
                .Select(movie => movie.Movie.VideoFiles)
                .Any()
        };
    }
    
    [HttpGet]
    [Route("{id:int}/watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Collections
            .AsNoTracking()
            .Where(collection => collection.Id == id)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.Library.LibraryUsers)
            
            .Where(collection => collection.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.Media
                        .Where(media => media.Type == "video"))
            
            .Include(collection => collection.Images
                .Where(image => image.Type == "logo"))
            
            .Include(collection => collection.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.Images)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.Translations
                        .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0])
                    )
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(collectionMovie => collectionMovie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
                        .ThenInclude(file => file.UserData)
            
            .FirstOrDefaultAsync();

        return collection is not null
            ? collection.CollectionMovies
                .Select((movie, index) => new PlaylistResponseDto(movie.Movie, index + 1))
                .ToArray()
            : [];
    }
    
    [HttpPost]
    [Route("{id:int}/like")]
    public async Task<StatusResponseDto<string>> Like(int id, [FromBody] LikeRequestDto request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Collections
            .AsNoTracking()
            .Where(collection => collection.Id == id)
            
            .FirstOrDefaultAsync();

        if (collection is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Collection not found"
            };
        }
        
        if(request.Like)
        {
            await mediaContext.CollectionUser.Upsert(new CollectionUser(collection.Id, userId))
                .On(m => new { m.CollectionId, m.UserId })
                .WhenMatched(m => new CollectionUser
                {
                    CollectionId = m.CollectionId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var collectionUser = await mediaContext.CollectionUser
                .Where(collectionUser => collectionUser.CollectionId == collection.Id && collectionUser.UserId == userId)
                .FirstOrDefaultAsync();
            
            if(collectionUser is not null)
            {
                mediaContext.CollectionUser.Remove(collectionUser);
            }
            
            await mediaContext.SaveChangesAsync();
        }
        
        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                collection.Title,
                request.Like ? "liked" : "unliked"
            }
        };
    }
}
