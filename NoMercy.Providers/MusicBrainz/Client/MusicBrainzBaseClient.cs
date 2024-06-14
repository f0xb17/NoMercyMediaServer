using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.MusicBrainz.Client;

public class MusicBrainzBaseClient : BaseClient
{
    protected override Uri BaseUrl => new("https://musicbrainz.org/ws/2/");
    protected override int ConcurrentRequests => 20;
    protected override int Interval => 1000;
    protected override string UserAgent => "anonymous";
    
    protected MusicBrainzBaseClient()
    {
        //
    }
    protected MusicBrainzBaseClient(Guid id) : base(id)
    {
        //
    }
}