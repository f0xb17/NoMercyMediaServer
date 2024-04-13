using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class TvImagesJob : IShouldQueue
{
    private readonly int _id;
    
    public TvImagesJob(long id)
    {
        _id = (int)id;
    }

    public async Task Handle()
    {
        await TvShowLogic.StoreImages(_id);
    }
}