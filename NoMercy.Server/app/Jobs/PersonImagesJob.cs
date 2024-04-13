using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class PersonImagesJob : IShouldQueue
{
    private readonly int _id;
    
    public PersonImagesJob(long id)
    {
        _id = (int)id;
    }

    public async Task Handle()
    {
        await PersonLogic.StoreImages(_id);
    }
}