// ReSharper disable All

using System.Diagnostics;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.FanArt.Models;

namespace NoMercy.Providers.FanArt.Client;

public class MovieClient : BaseClient
{
    public MovieClient(Guid id) : base(id)
    {
        AcoustID.Configuration.ClientKey = ApiInfo.AcousticId;
    }
    
    public Task<Movie?> Movie(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Movie>("movies/" + id, queryParams, priority);
    }
    
    public Task<Latest[]?> Latest(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Latest[]>("movies/latest" + id, queryParams, priority);
    }

}
