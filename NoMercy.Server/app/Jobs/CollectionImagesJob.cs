using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class CollectionImagesJob : IShouldQueue
{
    public int Id { get; set; }

    public CollectionImagesJob()
    {
        //
    }

    public CollectionImagesJob(int id)
    {
        Id = id;
    }

    public async Task Handle()
    {
        await CollectionLogic.StoreImages(Id);
    }
}