using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class MovieImagesJob : IShouldQueue
{
    public int Id { get; set; }

    public MovieImagesJob()
    {
        //
    }

    public MovieImagesJob(int id)
    {
        Id = id;
    }

    public async Task Handle()
    {
        await MovieLogic.StoreImages(Id);
    }
}