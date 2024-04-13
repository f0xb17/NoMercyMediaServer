using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.Other;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class AddShowJob : IShouldQueue
{
    private readonly int _id;
    private readonly string? _libraryId;
    
    public AddShowJob(long id)
    {
        _id = (int)id;
    }
    
    public AddShowJob(long id, string libraryId)
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
            TvClient tvClient = new(_id);
            TvShowDetails? tvShowDetails = await tvClient.WithAllAppends();
            if (tvShowDetails is null) return;
            
            string searchName = string.IsNullOrEmpty(tvShowDetails.OriginalName) 
                ? tvShowDetails.Name 
                : tvShowDetails.OriginalName;
            
            bool isAnime = await KitsuIo.IsAnime(searchName, tvShowDetails.FirstAirDate.ParseYear());
            
            library = await context.Libraries
                .Where(f => f.Type == (isAnime ? "anime" : "tv"))
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

        TvShowLogic tvShow = new(_id, library);
        await tvShow.Process();
        
        if (tvShow.Show != null)
        {
            Logger.MovieDb($@"TvShow {tvShow.Show.Name}: Processed");
            
            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = [ "tv", tvShow.Show.Id.ToString() ]
            });
        }

        tvShow.Dispose();
    }
}