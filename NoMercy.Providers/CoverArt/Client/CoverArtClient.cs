// ReSharper disable All

using System.Diagnostics;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.CoverArt.Models;

namespace NoMercy.Providers.CoverArt.Client;

public class CoverArtClient : BaseClient
{
    public CoverArtClient(Guid id) : base(id)
    {
        AcoustID.Configuration.ClientKey = ApiInfo.AcousticId;
    }
    
    public Task<Covers?> Cover(bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Covers>("release/" + Id, queryParams, priority);
    }
    
}
