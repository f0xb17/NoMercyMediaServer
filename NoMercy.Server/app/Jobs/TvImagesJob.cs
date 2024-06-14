using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class TvImagesJob : IShouldQueue
{
    public int Id { get; set; }

    public TvImagesJob()
    {
        //
    }

    public TvImagesJob(int id)
    {
        Id = id;
    }

    public async Task Handle()
    {
        await TvShowLogic.StoreImages(Id);
    }
}