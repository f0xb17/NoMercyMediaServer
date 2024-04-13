using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class CollectionImagesJob : IShouldQueue
{
    private readonly int _id;
    
    public CollectionImagesJob(long id)
    {
        _id = (int)id;
    }
    
    public async Task Handle()
    {
        await CollectionLogic.StoreImages(_id);
    }
}