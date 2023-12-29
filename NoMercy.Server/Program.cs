using Microsoft.AspNetCore;
using ElectronNET.API;
using NoMercy.Server.Helpers;

namespace NoMercy.Server;

public abstract class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        AppFiles.CreateAppFolders();
        Networking.Discover();
        Auth.Init();
        
        Task.Run(async () =>
        {
            await ApiInfo.RequestInfo();
            await Register.Init();
        }).Wait();

        Task.Run(async () =>
        {
            await Binaries.DownloadAll();

            Task.Delay(20000).Wait();
            Console.WriteLine($"Internal Address: {Networking.InternalAddress}");
            Console.WriteLine($"External Address: {Networking.ExternalAddress}");
        });
            
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureKestrel(Certificate.KestrelConfig)
            .UseElectron(args)
            .UseUrls("https://0.0.0.0:7626")
            .UseStartup<Startup>();
    }
}