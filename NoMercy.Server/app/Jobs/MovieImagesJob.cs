using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class MovieImagesJob : IShouldQueue
{
    private readonly int _id;
    
    public MovieImagesJob(long id)
    {
        _id = (int)id;
    }
    
    public async Task Handle()
    {
        await MovieLogic.StoreImages(_id);
    }
}