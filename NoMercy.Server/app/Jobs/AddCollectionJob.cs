using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class AddCollectionJob : IShouldQueue
{
    public int Id { get; set; }
    public Library? Library { get; set; }

    public AddCollectionJob()
    {
        //
    }

    public AddCollectionJob(int id, Library? library)
    {
        Id = id;
        Library = library;
    }

    public async Task Handle()
    {
        await using MediaContext context = new();

        if (Library is null) return;

        await using CollectionLogic collection = new(Id, Library);
        await collection.Process();

        if (collection.Collection != null)
        {
            Logger.MovieDb($@"Movie {collection.Collection.Name}: Processed");

            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["collection", collection.Collection.Id.ToString()]
            });
        }
    }
}