using NoMercy.Queue.system;
using NoMercy.Server.Logic;

namespace NoMercy.Server.app.Jobs;

public class ImagesJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _type;
    
    public ImagesJob(long id, string type)
    {
        _id = (int)id;
        _type = type;
    }

    public async Task Handle()
    {
        switch (_type)
        {
            case "tv":
                await TvShowLogic.StoreImages(_id);
                break;
            case "movie":
                await MovieLogic.StoreImages(_id);
                break;
            case "collection":
                await CollectionLogic.StoreImages(_id);
                break;
        }
    }
}