using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.CoverArt.Client;

public class CoverArtBaseClient : BaseClient
{
    protected override Uri BaseUrl => new("https://coverartarchive.org/");
    protected override int ConcurrentRequests => 3;
    protected override int Interval => 1000;
    
    protected CoverArtBaseClient(Guid id) : base(id)
    {
        //
    }
}