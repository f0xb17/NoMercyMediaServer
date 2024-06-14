using System.Diagnostics;
using CommandLine;
using Microsoft.AspNetCore;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Providers.AniDb.Clients;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;
using Networking = NoMercy.Server.app.Helper.Networking;

namespace NoMercy.Server;

public static class Program
{
    public static Task Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
        {
            var exception = (Exception)eventArgs.ExceptionObject;
            Logger.App("UnhandledException " + exception);
        };

        return Parser.Default.ParseArguments<StartupOptions>(args)
            .MapResult(Start, ErrorParsingArguments);

        static Task ErrorParsingArguments(IEnumerable<Error> errors)
        {
            Environment.ExitCode = 1;
            return Task.CompletedTask;
        }
    }

    private static async Task Start(StartupOptions options)
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        Databases.QueueContext = new QueueContext();
        Databases.MediaContext = new MediaContext();

        Init();

        var app = CreateWebHostBuilder(new WebHostBuilder()).Build();

        app.Services.GetService<IHostApplicationLifetime>()?.ApplicationStarted.Register(() =>
        {
            Task.Run(() =>
            {
                stopWatch.Stop();

                Task.Delay(300).Wait();

                Logger.App($@"Internal Address: {Networking.InternalAddress}");
                Logger.App($@"External Address: {Networking.ExternalAddress}");

                ConsoleMessages.ServerRunning();

                Logger.App($@"Server started in {stopWatch.ElapsedMilliseconds}ms");
            });
        });

        new Thread(() => app.RunAsync()).Start();
        new Thread(Dev.Run).Start();

        await Task.CompletedTask;
    }

    private static async Task Shutdown()
    {
        await Task.CompletedTask;
    }

    private static async Task Restart()
    {
        await Task.CompletedTask;
    }

    private static IWebHostBuilder CreateWebHostBuilder(this IWebHostBuilder _)
    {
        return WebHost.CreateDefaultBuilder([])
            .ConfigureKestrel(Certificate.KestrelConfig)
            .UseUrls("https://0.0.0.0:" + SystemInfo.ServerPort)
            .UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.Limits.MaxRequestBodySize = null;
                options.Limits.MaxRequestBufferSize = null;
                options.Limits.MaxConcurrentConnections = null;
                options.Limits.MaxConcurrentUpgradedConnections = null;
            })
            .UseQuic()
            .UseSockets()
            .UseStartup<Startup>();
    }

    private static void Init()
    {
        ApiInfo.RequestInfo().Wait();
        List<Task> startupTasks =
        [
            ConsoleMessages.Logo(),
            AppFiles.CreateAppFolders(),
            Networking.Discover(),
            Auth.Init(),
            Seed.Init(),
            Register.Init(),
            Binaries.DownloadAll(),
            new Task(() => QueueRunner.Initialize().Wait()),
            // AniDbBaseClient.Init(),
            TrayIcon.Make()
        ];

        AppDomain.CurrentDomain.ProcessExit += (_, _) => { AniDbBaseClient.Dispose(); };

        RunStartup(startupTasks);
    }

    private static void RunStartup(List<Task> startupTasks)
    {
        foreach (var task in startupTasks)
        {
            if (task.IsCompleted) continue;
            task.ConfigureAwait(false);
            task.Start();
        }
    }
}