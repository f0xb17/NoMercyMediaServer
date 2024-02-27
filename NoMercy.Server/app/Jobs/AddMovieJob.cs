using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class AddMovieJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _libraryId;
    
    public AddMovieJob(long id, string libraryId)
    {
        _id = (int)id;
        _libraryId = libraryId;
    }

    public async Task Handle()
    {
        Logger.App($"AddMovieJob Handle {_id} {_libraryId}");
        await using MediaContext context = new MediaContext();
        Library? library = await context.Libraries
            .Where(f => f.Id == Ulid.Parse(_libraryId))
            .Include(l => l.FolderLibraries)
            .ThenInclude(fl => fl.Folder)
            .FirstOrDefaultAsync();
        
        if (library is null) return;

        MovieLogic movie = new(_id, library);
        await movie.Process();
        if (movie.Movie != null)
        {
            Logger.MovieDb($@"Movie {movie.Movie.Title}: Processed");
            Console.WriteLine("");
        }

        movie.Dispose();
    }
}