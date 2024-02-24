using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Queue.system;
using NoMercy.Server.Logic;

namespace NoMercy.Server.app.Jobs;

public class CollectionJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _libraryId;
    
    public CollectionJob(long id, string libraryId)
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
        
        CollectionLogic collection = new(_id, library);
        await collection.Process();
        
        if (collection.Collection != null)
        {
            Logger.MovieDb($@"Movie {collection.Collection.Name}: Processed");
            Console.WriteLine("");
        }

        collection.Dispose();
    }
}