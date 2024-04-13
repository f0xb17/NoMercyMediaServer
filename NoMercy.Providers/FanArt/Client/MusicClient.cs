// ReSharper disable All

using System.Diagnostics;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Providers.FanArt.Models;

namespace NoMercy.Providers.FanArt.Client;

public class MusicClient : BaseClient
{
    public MusicClient() : base()
    {
        AcoustID.Configuration.ClientKey = ApiInfo.AcousticId;
    }
    
    public Task<ArtistDetails?> Artist(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<ArtistDetails>("music/" + id, queryParams, priority);
    }
    
    public Task<Album?> Album(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Album>("music/albums/" + id, queryParams, priority);
    }
    
    public Task<Label?> Label(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Label>("music/labels/" + id, queryParams, priority);
    }
    
    public Task<Latest[]?> Latest(Guid id, bool priority = false)
    {
        var queryParams = new Dictionary<string, string>();
        
        return Get<Latest[]>("music/latest", queryParams, priority);
    }

}
