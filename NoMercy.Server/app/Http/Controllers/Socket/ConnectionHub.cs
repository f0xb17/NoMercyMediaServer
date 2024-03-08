#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Collections.Concurrent;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace NoMercy.Server.app.Http.Controllers.Socket;

[Authorize]
public class ConnectionHub : Hub
{
    private static readonly ConcurrentDictionary<string, Client> _clients = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected ConnectionHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public override async Task OnConnectedAsync()
    {
    string? id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    Guid userId = Guid.Parse(id ?? string.Empty);
    
    await using MediaContext mediaContext = new();
    User? user = await mediaContext.Users.FindAsync(userId);
    
    if(user is null)
    {
        return;
    }
    
    var accessToken = _httpContextAccessor.HttpContext?.Request.Query.FirstOrDefault(x => x.Key == "access_token").Value;
    string[] result = accessToken.GetValueOrDefault().ToString().Split("&");
    
    Client client = new Client
    {
        Sub = user.Id,
        Ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
    };
    
    foreach (var item in result)
    {
        string[] keyValue = item.Split("=");
        
        if(keyValue.Length < 2)
        {
            continue;
        }
        
        keyValue[1] = keyValue[1].Replace("+", " ");
        
        switch (keyValue[0])
        {
            case "access_token":
                continue;
            case "client_id":
                client.DeviceId = keyValue[1];
                break;
            case "client_name":
                client.Name = keyValue[1];
                break;
            case "client_type":
                client.Type = keyValue[1];
                break;
            case "client_version":
                client.Version = keyValue[1];
                break;
            case "client_os":
                client.Os = keyValue[1];
                break;
            case "client_browser":
                client.Browser = keyValue[1];
                break;
            case "client_device":
                client.Model = keyValue[1];
                break;
        }
    }
    
    await mediaContext.Devices.Upsert(client)
        .On(x => x.DeviceId)
        .WhenMatched((ds, di) => new Device
        {
            Browser = di.Browser,
            CustomName = di.CustomName,
            DeviceId = di.DeviceId,
            Ip = di.Ip,
            Model = di.Model,
            Name = di.Name,
            Os = di.Os,
            Type = di.Type,
            Version = di.Version,
        })
        .RunAsync();
    
    Device? device = mediaContext.Devices.FirstOrDefault(x => x.DeviceId == client.DeviceId);
    
    if(device is not null)
    {
        await mediaContext.ActivityLogs.AddAsync(new ActivityLog
        {
            DeviceId = device.Id,
            Time = DateTime.Now,
            Type = "Connected to server",
            UserId = user.Id,
        });
        
        await mediaContext.SaveChangesAsync();
    }
    
    _clients.TryAdd(Context.ConnectionId, client);
    

        await base.OnConnectedAsync();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if(_clients.TryGetValue(Context.ConnectionId, out Client? client))
        {
            await using MediaContext mediaContext = new();
            Device? device = mediaContext.Devices.FirstOrDefault(x => x.DeviceId == client.DeviceId);
            if(device is not null)
            {
                await mediaContext.ActivityLogs.AddAsync(new ActivityLog
                {
                    DeviceId = device.Id,
                    Time = DateTime.Now,
                    Type = "Disconnected from server",
                    UserId = client.Sub,
                });
            
                await mediaContext.SaveChangesAsync();
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
}

public class ClientRequest
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
}

public class Client: Device
{
    public Guid Sub { get; set; }
    public int? Ping { get; set; }
}