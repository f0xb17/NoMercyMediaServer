using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class AddShowJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _libraryId;
    
    public AddShowJob(long id, string libraryId)
    {
        _id = (int)id;
        _libraryId = libraryId;
    }

    public async Task Handle()
    {
        await using MediaContext context = new MediaContext();

        Library? library = await context.Libraries
            .Where(f => f.Id == Ulid.Parse(_libraryId))
            .Include(l => l.FolderLibraries)
            .ThenInclude(fl => fl.Folder)
            .FirstOrDefaultAsync();
        
        if (library is null) return;

        TvShowLogic tvShow = new(_id, library);
        await tvShow.Process();
        
        if (tvShow.Show != null)
        {
            Logger.MovieDb($@"TvShow {tvShow.Show.Name}: Processed");
            Console.WriteLine("");
        }

        tvShow.Dispose();
    }
}