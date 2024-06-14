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

[Serializable]
public class AddShowJob : IShouldQueue, IDisposable, IAsyncDisposable
{
    public int Id { get; set; }
    public Library? Library { get; set; }

    public AddShowJob()
    {
        //
    }

    public AddShowJob(int id)
    {
        Id = id;
    }

    public AddShowJob(int id, Library? library = null)
    {
        Id = id;
        Library = library;
    }

    public async Task Handle()
    {
        await using MediaContext context = new();

        if (Library is null) return;

        await using TvShowLogic tvShow = new(Id, Library);
        await tvShow.Process();

        if (tvShow.Show != null)
        {
            Logger.MovieDb($@"TvShow {tvShow.Show.Name}: Processed");

            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["tv", tvShow.Show.Id.ToString()]
            });
        }
    }

    public void Dispose()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public async ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }
}