using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class SeasonImagesJob : IShouldQueue
{
    public int Id { get; set; }
    public int SeasonNumber { get; set; }

    public SeasonImagesJob()
    {
        //
    }

    public SeasonImagesJob(int id, int seasonNumber)
    {
        Id = id;
        SeasonNumber = seasonNumber;
    }

    public async Task Handle()
    {
        await SeasonLogic.StoreImages(Id, SeasonNumber);
    }
}