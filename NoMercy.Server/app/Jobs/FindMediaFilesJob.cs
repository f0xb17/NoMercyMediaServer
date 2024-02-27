using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
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
        await using MediaContext context = new MediaContext();
        Library? library = await context.Libraries
            .Where(f => f.Id == Ulid.Parse(_libraryId))
            .Include(l => l.FolderLibraries)
            .ThenInclude(fl => fl.Folder)
            .FirstOrDefaultAsync();
        
        if (library == null) return;
        
        FileLogic file = new(_id, library);
        await file.Process();
        
        if (file.Files.Count > 0)
        {
            Logger.MovieDb($@"Found {file.Files.Count} files in {file.Files.FirstOrDefault()?.Path}");
            Console.WriteLine("");
        }

        file.Dispose();
    }
}