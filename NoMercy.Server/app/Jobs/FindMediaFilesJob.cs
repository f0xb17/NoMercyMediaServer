using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class FindMediaFilesJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _libraryId;
    
    public FindMediaFilesJob(long id, string libraryId)
    {
        _id = (int)id;
        _libraryId = libraryId;
    }

    public async Task Handle()
    {
        Logger.Queue($"Finding media files for {_id} in library {_libraryId}");
        
        await using MediaContext context = new MediaContext();
        Library? library = await context.Libraries
            .AsTracking()
            
            .Where(f => f.Id == Ulid.Parse(_libraryId))
            
            .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
            
            .Include(l => l.LibraryMovies
                .Where(lm => lm.Movie.Id == _id)
            )
                .ThenInclude(lm => lm.Movie)
            
            .Include(l => l.LibraryTvs
                .Where(lt => lt.Tv.Id == _id)
            )
                .ThenInclude(lt => lt.Tv)
            
            .FirstOrDefaultAsync();
        
        Logger.Queue($"Found library {library?.Id} with {library?.FolderLibraries.Count} folders");
        
        if (library == null) return;
        
        FileLogic file = new(_id, library);
        await file.Process();
        
        if (file.Files.Count > 0)
        {
            Logger.MovieDb($@"Found {file.Files.Count} files in {file.Files.FirstOrDefault()?.Path}");
            Console.WriteLine("");
            
            // if(library.LibraryMovies.Count > 0)
            // {
            //     LibraryMovie? libraryMovie = library.LibraryMovies?.FirstOrDefault();
            //     if (libraryMovie == null) return;
            //     
            //     libraryMovie.Movie.Folder = file.Files.FirstOrDefault()?.Path;
            //
            //     await context.SaveChangesAsync();
            //     
            // }
            // else if(library.LibraryTvs.Count > 0)
            // {
            //     LibraryTv? libraryTv = library.LibraryTvs?.FirstOrDefault();
            //     if (libraryTv == null) return;
            //     
            //     libraryTv.Tv.Folder = file.Files.FirstOrDefault()?.Path;
            //     
            //     await context.SaveChangesAsync();
            //     
            // }
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = [ "libraries", library.Id.ToString() ]
            });
        }
        
        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
        {
            QueryKey = [ library.Type == "movie" ? "movies" : "tvs", _id]
        });
        
        file.Dispose();
    }

}

public class RefreshLibraryDto
{
    [JsonProperty("queryKey")]
    public dynamic[] QueryKey { get; set; } = [];
}