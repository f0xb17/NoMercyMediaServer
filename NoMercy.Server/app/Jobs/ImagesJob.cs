using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class ImagesJob : IShouldQueue
{
    private readonly int _id;
    private readonly int _seasonNumber;
    private readonly int _episodeNumber;
    private readonly string _type;
    
    public ImagesJob(long id, string type)
    {
        _id = (int)id;
        _type = type;
    }
    
    public ImagesJob(long id, string type, long seasonNumber)
    {
        _id = (int)id;
        _type = type;
        _seasonNumber = (int)seasonNumber;
    }
    
    public ImagesJob(long id, string type, long seasonNumber, long episodeNumber)
    {
        _id = (int)id;
        _type = type;
        _seasonNumber = (int)seasonNumber;
        _episodeNumber = (int)episodeNumber;
    }

    public async Task Handle()
    {
        switch (_type)
        {
            case "tv":
                await TvShowLogic.StoreImages(_id);
                break;
            case "season":
                await SeasonLogic.StoreImages(_id, _seasonNumber);
                break;
            case "episode":
                await EpisodeLogic.StoreImages(_id, _seasonNumber, _episodeNumber);
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