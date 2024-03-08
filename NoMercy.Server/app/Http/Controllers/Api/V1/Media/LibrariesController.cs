using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Libraries")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/libraries")]
public class LibrariesController : Controller
{
    [HttpGet]
    public async Task<LibrariesResponseDto> Libraries()
    {       
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var libraries = await mediaContext.Libraries
            .AsNoTracking()
            
            .Where(library => library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null
            )
            
            .Include(library => library.FolderLibraries)
                .ThenInclude(folderLibrary => folderLibrary.Folder)
                    .ThenInclude(folder => folder.EncoderProfileFolder)
                        .ThenInclude(library => library.EncoderProfile)
            
            .Include(library => library.LanguageLibraries)
                .ThenInclude(languageLibrary => languageLibrary.Language)
            
            .OrderBy(library => library.Order)
            .ToListAsync();

        return new LibrariesResponseDto
        {
            Data = libraries
                .Select(library => new LibrariesResponseItemDto(library))
        };
    }
    
    [HttpGet]
    [Route("{libraryId}")]
    public async Task<LibraryResponseDto> Library(Ulid libraryId, [FromQuery] PageRequestDto requestDto)
    {        
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var library = await mediaContext.Libraries
            .AsNoTracking()
            
            .Where(library => library.Id == libraryId)
            .Where(library => library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null
            )
            
            .Take(requestDto.Take)
            .Skip(requestDto.Page * requestDto.Take)
            
            .Include(library => library.LibraryMovies
                .Where(libraryMovie => libraryMovie.Movie.VideoFiles
                    .Any(videoFile => videoFile.Folder != null) == true
                )
            )
                .ThenInclude(libraryMovie => libraryMovie.Movie)
                    .ThenInclude(movie => movie.VideoFiles
                        .Where(videoFile => videoFile.Folder != null)
                    )
            
            .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Media)
            
            .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Images)
            
            .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.GenreMovies)
                    .ThenInclude(genreMovie => genreMovie.Genre)
            
            .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.Translations
                    .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(library => library.LibraryMovies)
                .ThenInclude(libraryMovie => libraryMovie.Movie.CertificationMovies)
                    .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(library => library.LibraryTvs
                .Where(libraryTv => libraryTv.Tv.Episodes
                    .Any(episode => episode.VideoFiles
                        .Any(videoFile => videoFile.Folder != null) == true
                    ) == true
                )
            )
                .ThenInclude(libraryTv => libraryTv.Tv)
                    .ThenInclude(tv => tv.Episodes)
                        .ThenInclude(episode => episode.VideoFiles
                            .Where(videoFile => videoFile.Folder != null)
                        )
            
            .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Media)
            
            .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Images)
            
            .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.GenreTvs)
                    .ThenInclude(genreTv => genreTv.Genre)
            
            .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.Translations
                    .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(library => library.LibraryTvs)
                .ThenInclude(libraryTv => libraryTv.Tv.CertificationTvs)
                    .ThenInclude(certificationTv => certificationTv.Certification)
            
            .FirstAsync();
        
        return new LibraryResponseDto
        {
            Data = library.LibraryMovies
                    .Select(movie => new LibraryResponseItemDto(movie))
                    
                    .Concat(library.LibraryTvs
                        .Select(tv => new LibraryResponseItemDto(tv)))
                    
                    .OrderBy(libraryResponseDto => libraryResponseDto.TitleSort),
            
            NextId = (library.LibraryMovies.Count + library.LibraryTvs.Count) < requestDto.Take
                ? null 
                : (library.LibraryMovies.Count + library.LibraryTvs.Count) + (requestDto.Page * requestDto.Take)
        };
    }

}