#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Mono.Nat;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Socket;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.app.Helper;

public class Networking
{
    private static IHubContext<ConnectionHub> HubContext { get; set; }

    public Networking(IHubContext<ConnectionHub> hubContext)
    {
        HubContext = hubContext;
    }

    public Networking()
    {
        // GetExternalIp();
    }

    private static INatDevice? _device;

    internal static readonly ConcurrentDictionary<string, Client> SocketClients = new();


    public static Task Discover()
    {
        NatUtility.DeviceFound += DeviceFound;

        NatUtility.StartDiscovery();

        return Task.CompletedTask;
    }

    private static string? _internalIp;

    public static string InternalIp
    {
        get => _internalIp ?? GetInternalIp();
        set => _internalIp = value;
    }

    private static string? _externalIp;

    private static string ExternalIp
    {
        get => _externalIp ?? GetExternalIp();
        set => _externalIp = value;
    }

    public static int InternalServerPort { get; set; } = 7626;
    public static int ExternalServerPort { get; set; } = 7626;

    public static string InternalAddress { get; private set; } = "";

    public static string ExternalAddress { get; private set; } = "";

    private static string GetInternalIp()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);

        socket.Connect("1.1.1.1", 65530);

        var endPoint = socket.LocalEndPoint as IPEndPoint;

        var localIp = endPoint?.Address.ToString().Replace("\"", "");

        if (localIp == null) return "";

        InternalAddress = $"https://{Regex.Replace(localIp, "\\.", "-")}.{SystemInfo.DeviceId}.nomercy.tv:7626";

        return localIp;
    }

    private static string GetExternalIp()
    {
        var client = new HttpClient();

        string externalIp = client.GetStringAsync("https://api-dev.nomercy.tv/server/ip").Result.Replace("\"", "");

        ExternalAddress = $"https://{Regex.Replace(externalIp, "\\.", "-")}.{SystemInfo.DeviceId}.nomercy.tv:7626";

        return externalIp;
    }

    private static void DeviceFound(object? sender, DeviceEventArgs args)
    {
        _device = args.Device;

        _device.CreatePortMap(new Mapping(Protocol.Tcp, 7626, 7626, 9999999, "NoMercy MediaServer (TCP)"));
        _device.CreatePortMap(new Mapping(Protocol.Udp, 7626, 7626, 9999999, "NoMercy MediaServer (UDP)"));

        ExternalIp = _device.GetExternalIP().ToString();

        if (ExternalIp == "")
        {
            ExternalIp = GetExternalIp();
        }
        else
        {
            ExternalAddress = $"https://{Regex.Replace(ExternalIp, "\\.", "-")}.{SystemInfo.DeviceId}.nomercy.tv:7626";
        }
    }

    public static IWebHost TempServer()
    {
        return WebHost.CreateDefaultBuilder()
            .UseUrls("http://0.0.0.0:" + InternalServerPort)
            .Configure(app =>
            {
                app.Run(async context =>
                {
                    var code = context.Request.Query["code"].ToString();

                    Auth.GetTokenByAuthorizationCode(code);

                    context.Response.Headers.Append("Content-Type", "text/html");
                    await context.Response.WriteAsync("<script>window.close();</script>");
                });
            }).Build();
    }

    internal static void SendToAll(string name, object? data = null)
    {
        foreach (var (_, client) in SocketClients)
        {
            try
            {
                if (data != null)
                    client.Socket.SendAsync(name, data).Wait();
                else
                    client.Socket.SendAsync(name).Wait();
            }
            catch (Exception e)
            {
                Logger.Socket(e, LogLevel.Error);
                throw;
            }
        }
    }

    private static void SendTo(string name, Guid userId, object? data = null)
    {
        foreach (var (_, client) in SocketClients.Where(client => client.Value.Sub == userId))
        {
            try
            {
                if (data != null)
                    client.Socket.SendAsync(name, data).Wait();
                else
                    client.Socket.SendAsync(name).Wait();
            }
            catch (Exception e)
            {
                Logger.Socket(e, LogLevel.Error);
                throw;
            }
        }
    }

    private static void Reply(string name, HttpContext context, object? data = null)
    {
        Guid userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        foreach (var (_, client) in SocketClients.Where(client => client.Value.Sub == userId))
        {
            try
            {
                if (data != null)
                    client.Socket.SendAsync(name, data).Wait();
                else
                    client.Socket.SendAsync(name).Wait();
            }
            catch (Exception e)
            {
                Logger.Socket(e, LogLevel.Error);
                throw;
            }
        }
    }
}