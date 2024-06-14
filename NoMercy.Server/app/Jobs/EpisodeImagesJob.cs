using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class EpisodeImagesJob : IShouldQueue
{
    public int Id { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }

    public EpisodeImagesJob()
    {
        //
    }

    public EpisodeImagesJob(int id, int seasonNumber, int episodeNumber)
    {
        Id = id;
        SeasonNumber = seasonNumber;
        EpisodeNumber = episodeNumber;
    }

    public async Task Handle()
    {
        await EpisodeLogic.StoreImages(Id, SeasonNumber, EpisodeNumber);
    }
}