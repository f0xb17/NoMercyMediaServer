using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class PersonImagesJob : IShouldQueue
{
    public int Id { get; set; }

    public PersonImagesJob()
    {
        //
    }

    public PersonImagesJob(int id)
    {
        Id = id;
    }

    public async Task Handle()
    {
        await PersonLogic.StoreImages(Id);
    }
}