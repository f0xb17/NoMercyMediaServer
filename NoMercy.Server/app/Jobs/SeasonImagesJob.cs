using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class SeasonImagesJob : IShouldQueue
{
    private readonly int _id;
    private readonly int _seasonNumber;
    
    public SeasonImagesJob(long id, long seasonNumber)
    {
        _id = (int)id;
        _seasonNumber = (int)seasonNumber;
    }
    
    public async Task Handle()
    {
        await SeasonLogic.StoreImages(_id, _seasonNumber);
    }
}