using System.Net.Http.Headers;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Helper;

public static class Register
{
    public static Task Init()
    {
        var serverData = new Dictionary<string, string>
        {
            {"server_id", SystemInfo.DeviceId.ToString()},
            {"server_name", SystemInfo.DeviceName},
            {"internal_ip", Networking.InternalIp},
            {"internal_port", Networking.InternalServerPort.ToString()},
            {"external_port", Networking.ExternalServerPort.ToString()},
            {"server_version", ApiInfo.ApplicationVersion},
            {"platform", SystemInfo.Platform}
        };
        
        Logger.Register(@"Registering Server, this takes a moment...");
        
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        var content = client.PostAsync("https://api-dev.nomercy.tv/v1/server/register", new FormUrlEncodedContent(serverData))
            .Result.Content.ReadAsStringAsync().Result;
        
        var data = JsonConvert.DeserializeObject(content);
        
        if (data == null) throw new Exception("Failed to register Server");
        
        Logger.Register(@"Server registered successfully");
        
        AssignServer().Wait();
        
        return Task.CompletedTask;
    }

    private static Task AssignServer()
    {
        var serverData = new Dictionary<string, string>
        {
            {"server_id", SystemInfo.DeviceId.ToString()}
        };
        
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

        var content = client.PostAsync("https://api-dev.nomercy.tv/v1/server/assign", new FormUrlEncodedContent(serverData))
            .Result.Content.ReadAsStringAsync().Result;
        
        var data = JsonConvert.DeserializeObject<ServerRegisterResponse>(content);
        
        if (data == null) throw new Exception("Failed to assign Server");
        
        Logger.Register(@"Server assigned successfully");
        
        Certificate.RenewSslCertificate().Wait();
        
        return Task.CompletedTask;
    }
}

public class ServerRegisterResponse
{
}