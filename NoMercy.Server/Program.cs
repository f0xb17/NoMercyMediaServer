using System.Diagnostics;
using System.Runtime.InteropServices;
using CommandLine;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.Helpers;
using NoMercy.Server.Logic;

namespace NoMercy.Server;

public static class Program
{ 
    public static Task Main(string[] args)
    {
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

        app.Services.GetService<IHostApplicationLifetime>()!.ApplicationStarted.Register(() =>
        {
            Task.Run(() =>
            {
                stopWatch.Stop();
                
                Task.Delay(200).Wait();
                
                Console.WriteLine(@"Internal Address: {0}", Networking.InternalAddress);
                Console.WriteLine(@"External Address: {0}", Networking.ExternalAddress);
                
                ConsoleMessages.ServerRunning();
                
                Console.WriteLine(@"Server started in {0}ms", stopWatch.ElapsedMilliseconds);
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
        Task.Run(async () =>
        {
            await ApiInfo.RequestInfo();
            ConsoleMessages.Logo();
        }).Wait();
        
        AppFiles.CreateAppFolders();
        Networking.Discover();
        Auth.Init();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18362))
        {
            TrayIcon _ = new();
        }

        Task.Run(async () =>
        {
            await Register.Init();
        }).Wait();

        Task.Run(async () =>
        {
            await Binaries.DownloadAll();
        });

        Task.Run(async () =>
        {
            var seed = new Seed();
            
            Library library = new()
            {
                Id = "cliryptr10002efmk294kpkhn",
                Title = "TV Shows",
                Type = "tv",
                Image = "/rfGg1ZLNvlfTfiY5941IFjEKMK1.jpg",
                SpecialSeasonName = "Specials",
                Country = "NL",
                Language = "nl",
                AutoRefreshInterval = 30,
                ChapterImages = true,
                ExtractChapters = true,
                ExtractChaptersDuring = true,
                Realtime = true,
            };
                
            await MediaContext.Db.Libraries.Upsert(library)
                .On(v => new { v.Id })
                .WhenMatched(v => new Library()
                {
                    UpdatedAt = DateTime.Now
                })
                .RunAsync();

            int[] ids =
            [
                44310,39340,1973,75219,30980,90546,30977,122587,75123,74092,77778,64537,75777,912,130237,65292,70637,61259,72519,1433,42942,74080,42589,63088,45348,62110,246,77529,41727,203780,61695,70716,50712,73223,12598,38464,82728,153657,99083,212963,95,66840,65931,102086,138357,74728,83962,71024,93256,80671,114410,66912,63145,87108,44006,58841,73017,1615,37527,72528,1404,76063,24835,72517,70636,31724,100567,58474,30991,72503,45859,46004,76122,85937,67026,86824,12577,75208,1405,4229,131927,73021,6326,60810,720,42410,54930,31682,1415,42671,196040,46261,1434,196285,90677,62273,87917,61456,1668,124364,39379,615,72426,1399,63453,70128,34793,76757,79166,31731,60730,60863,34749,1126,88329,88987,80665,45950,56998,110070,1408,19716,1425,1100,46298,232125,40178,71007,82738,158307,119335,46025,40424,62745,68080,74096,45799,72305,83054,46374,37585,2345,70593,74097,45998,2384,72635,60831,66080,8358,53715,69293,84958,72026,86831,63174,72636,72788,39483,60732,85324,83043,114695,102693,61550,1403,69088,73919,59427,66190,61889,63181,133903,40044,68716,62127,38472,62126,111312,92788,70784,67466,72705,62285,67178,69291,21732,21730,46195,92749,81358,62560,72296,65930,62565,2317,77723,1428,4614,124271,17610,61387,95317,890,80350,62640,60808,64710,92830,29117,39272,2632,69298,64394,56570,64196,77701,22254,1877,19780,20177,60572,8392,43865,62816,45502,42510,74163,113687,96316,60625,60845,71790,71789,42444,62742,28061,6005,64434,114472,34805,61664,1087,71018,92783,66857,50825,2190,26707,45844,42509,78102,26563,37680,117933,100414,45782,78204,72525,75975,114937,48561,66077,195819,68639,53347,48866,40546,1481,64671,1418,46952,68018,15632,45997,67676,81046,95205,88396,1996,69629,71712,72844,62430,2362,100088,33880,118663,82856,80670,5920,61367,15621,50325,78471,76719,83095,79744,90802,62104,456,47640,68590,64163,56351,4429,34742,205308,68101,46331,66926,75214,137,64414,85271,117821,119051,128010,63247,91363,86836,85844,71728,61663,80504,60866,83105
            ];
            
            foreach (var id in ids)
            {
                TvShowLogic tvShow = new(id, library);
                await tvShow.Process();
                if(tvShow.Show != null)
                    Console.WriteLine(@"{0} - {1}", id, tvShow.Show.Name);
            }
            
        });

        return WebHost.CreateDefaultBuilder([])
            .ConfigureKestrel(Certificate.KestrelConfig)
            .UseUrls("https://0.0.0.0:7626")
            .UseStartup<Startup>();
    }
}