using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class AddCollectionJob : IShouldQueue
{
    private readonly int _id;
    private readonly string? _libraryId;
    
    public AddCollectionJob(long id)
    {
        _id = (int)id;
    }
    
    public AddCollectionJob(long id, string libraryId)
    {
        _id = (int)id;
        _libraryId = libraryId;
    }

    public async Task Handle()
    {
        await using MediaContext context = new MediaContext();
        
        Library? library;
        
        if (_libraryId is null)
        {
            library = await context.Libraries
                .Where(f => f.Type == "movie")
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstOrDefaultAsync();
        } 
        else {
            library = await context.Libraries
                .Where(f => f.Id == Ulid.Parse(_libraryId))
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstOrDefaultAsync();
        }
        
        if (library is null) return;
        
        CollectionLogic collection = new(_id, library);
        await collection.Process();
        
        if (collection.Collection != null)
        {
            Logger.MovieDb($@"Movie {collection.Collection.Name}: Processed");
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = [ "collection", collection.Collection.Id.ToString() ]
            });
        }

        collection.Dispose();
    }
}