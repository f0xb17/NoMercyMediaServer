using System.Diagnostics;
using CommandLine;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
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
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var app = CreateWebHostBuilder(new WebHostBuilder()).Build();

        app.Services.GetService<IHostApplicationLifetime>()?.ApplicationStarted.Register(() =>
        {
            Task.Run(() =>
            {
                stopWatch.Stop();

                Task.Delay(200).Wait();

                Logger.App($@"Internal Address: {Networking.InternalAddress}");
                Logger.App($@"External Address: {Networking.ExternalAddress}");

                ConsoleMessages.ServerRunning();

                Logger.App($@"Server started in {stopWatch.ElapsedMilliseconds}ms");
            });
        });

        app.RunAsync().Wait();

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

    private static IWebHostBuilder CreateWebHostBuilder(this IWebHostBuilder builder)
    {
        ApiInfo.RequestInfo().Wait();
        List<Task> startupTasks =
        [
            ConsoleMessages.Logo(),
            AppFiles.CreateAppFolders(),
            Networking.Discover(),
            Auth.Init(),
            Register.Init(),
            Binaries.DownloadAll(),
            Seed.Init(),
            Task.Run(() => QueueRunner.Initialize().Wait()),
            TrayIcon.Make()
        ];
        RunStartup(startupTasks).Wait();

        // Task.Run(async () =>
        // {
        //     MediaContext context = new();
        //     var library = await context.Libraries
        //         .Where(library => library.Title == "Films")
        //         .Include(l => l.FolderLibraries)
        //         .ThenInclude(fl => fl.Folder)
        //         .FirstOrDefaultAsync();
        //
        //     if (library == null) return;
        //
        //     List<Movie> movies = await context.Movies
        //         .Where(movie => movie.LibraryId == library.Id)
        //         .Where(movie => movie.Folder != null && movie.Folder != "")
        //         .OrderBy(movie => movie.TitleSort)
        //         .ToListAsync();
        //
        //     if (movies.Count == 0) return;
        //
        //     foreach (var movie in movies)
        //     {
        //         Logger.App($@"Processing {movie.Title}");
        //         
        //         AddMovieJob addMovieJob = new(movie.Id, library.Id.ToString());
        //         await addMovieJob.Handle();
        //
        //         FindMediaFilesJob findMediaFilesJob = new(movie.Id, library.Id.ToString());
        //         await findMediaFilesJob.Handle();
        //     }
        // });
        
        // Task.Run(async () =>
        // {
        //     MediaContext context = new();
        //     var library = await context.Libraries
        //         .Where(library => library.Type == "tv")
        //         .Include(l => l.FolderLibraries)
        //         .ThenInclude(fl => fl.Folder)
        //         .FirstOrDefaultAsync();
        //
        //     if (library == null) return;
        //     
        //     SearchClient searchClient = new();
        //     var paginatedTvShowResponse = await searchClient.TvShow("Avatar The Last Airbender", "2005");
        //
        //     if (paginatedTvShowResponse?.Results.Length <= 0) return;
        //         
        //     // List<TvShow> res = Str.SortByMatchPercentage(paginatedTvShowResponse.Results, m => m.Name, folder.Parsed.Title);
        //     List<TvShow> res = paginatedTvShowResponse?.Results.ToList() ?? [];
        //     if (res.Count is 0) return;
        //     
        //     Logger.App($@"Processing {res[0].Name}");
        //     
        //     AddShowJob addShowJob = new AddShowJob(id:res[0].Id, libraryId:library.Id.ToString());
        //     JobDispatcher.Dispatch(addShowJob, "queue", 10);
        //     //
        //     // List<Tv> tvs = await context.Tvs
        //     //     .Where(tv => tv.LibraryId == library.Id)
        //     //     .Where(tv => tv.Folder != null && tv.Folder != "")
        //     //     .Where(tv => tv.Id == 82452)
        //     //     .OrderBy(tv => tv.TitleSort)
        //     //     .ToListAsync();
        //     //
        //     // if (tvs.Count == 0) return;
        //     //
        //     // foreach (var tv in tvs)
        //     // {
        //     //     Logger.App($@"Processing {tv.Title}");
        //     //     
        //     //     AddShowJob addShowJob = new(tv.Id, library.Id.ToString());
        //     //     await addShowJob.Handle();
        //     //     
        //     //     FindMediaFilesJob findMediaFilesJob = new(tv.Id, library.Id.ToString());
        //     //     await findMediaFilesJob.Handle();
        //     // }
        // });
        
        return WebHost.CreateDefaultBuilder([])
            .ConfigureKestrel(Certificate.KestrelConfig)
            .UseUrls("https://0.0.0.0:" + SystemInfo.ServerPort)
            .UseKestrel(options => options.AddServerHeader = false)
            .UseQuic()
            .UseStartup<Startup>();
    }

    private static async Task RunStartup(List<Task> startupTasks)
    {
        foreach (var task in startupTasks)
        {
            await task;
        }
    }
}