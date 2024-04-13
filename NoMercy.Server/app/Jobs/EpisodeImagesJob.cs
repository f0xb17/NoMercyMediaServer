using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class EpisodeImagesJob : IShouldQueue
{
    private readonly int _id;
    private readonly int _seasonNumber;
    private readonly int _episodeNumber;
    
    public EpisodeImagesJob(long id, long seasonNumber, long episodeNumber)
    {
        _id = (int)id;
        _seasonNumber = (int)seasonNumber;
        _episodeNumber = (int)episodeNumber;
    }

    public async Task Handle()
    {
        await EpisodeLogic.StoreImages(_id, _seasonNumber, _episodeNumber);
    }
}