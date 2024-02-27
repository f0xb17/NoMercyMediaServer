using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Collections")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/collections")]
public class CollectionsController : Controller
{
    [HttpGet]
    public async Task<CollectionsResponseDto> Libraries()
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
            .Include(collection => collection.Translations)
            
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
    public async Task<CollectionResponseDto> Tv(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var collection = await mediaContext.Collections
            .AsNoTracking()
            
            .Where(collection => collection.Id == id)
            
            .Where(collection => collection.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            
            .Include(collection => collection.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.Translations)
            
            .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
            
            // .Include(collection => collection.Translations
            //     .Where(translation => translation.Iso6391 == "en" || translation.Iso6391 == "nl"))
            
            .Include(collection => collection.Images
                // .Where(image => 
                //     (image.Type == "logo" && (image.Iso6391 == "en" || image.Iso6391 == null))
                //     || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                // )
            )

            .FirstOrDefaultAsync();
        
        return new CollectionResponseDto
        {
            Data = collection is not null 
                ? new CollectionResponseItemDto(collection) 
                : null
        };
    }
}
