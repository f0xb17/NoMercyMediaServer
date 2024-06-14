using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public class LibraryLogic(Ulid id) : IDisposable, IAsyncDisposable
{
    private readonly MediaContext _mediaContext = new();
    private Library Library { get; set; } = new();

    public Ulid Id { get; set; } = id;

    private int Depth { get; set; }

    public List<dynamic> Titles { get; } = [];
    private List<string> Paths { get; } = [];

    public async Task<bool> Process()
    {
        var library = await _mediaContext.Libraries
            .AsNoTracking()
            .Include(library => library.FolderLibraries)
                .ThenInclude(folderLibrary => folderLibrary.Folder)
            .FirstOrDefaultAsync(library => library.Id == Id);

        if (library is null) return false;

        Library = library;

        Paths.AddRange(Library.FolderLibraries
            .Select(folderLibrary => folderLibrary.Folder.Path));

        GetDepth();

        await ScanFolder();

        await Store();

        return true;
    }

    private async Task Store()
    {
        await Task.CompletedTask;
    }

    private void GetDepth()
    {
        Depth = Library.Type switch
        {
            "movie" => 1,
            "tv" => 1,
            "music" => 3,
            _ => 1
        };
    }

    private async Task ScanFolder()
    {
        foreach (var path in Paths)
            switch (Library?.Type)
            {
                case "music":
                    await ScanAudioFolder(path);
                    break;
                default:
                    await ScanVideoFolder(path);
                    break;
            }

        Logger.App("Scanning done");
    }

    private async Task ScanVideoFolder(string path)
    {
        await using MediaScan mediaScan = new();
        var list = await mediaScan
            .Process(path, Depth);

        foreach (var folder in list) await ProcessVideoFolder(folder);

        Titles.AddRange(list);
    }

    private async Task ProcessVideoFolder(MediaFolder path)
    {
        switch (Library.Type)
        {
            case "movie":
            {
                await ProcessMovieFolder(path);
                break;
            }
            case "tv":
            {
                await ProcessTvFolder(path);
                break;
            }
        }
    }

    private async Task ProcessMovieFolder(MediaFolder folder)
    {
        if (folder.Parsed is null) return;

        await using TmdbSearchClient tmdbSearchClient = new();
        var paginatedMovieResponse = await tmdbSearchClient.Movie(folder.Parsed.Title, folder.Parsed.Year);

        if (paginatedMovieResponse?.Results.Length <= 0) return;

        // List<Movie> res = Str.SortByMatchPercentage(paginatedMovieResponse?.Results, m => m.Title, folder.Parsed.Title);
        var res = paginatedMovieResponse?.Results.ToList() ?? [];
        if (res.Count is 0) return;

        Titles.Add(res[0].Title);

        AddMovieJob addMovieJob = new(res[0].Id, Library);
        JobDispatcher.Dispatch(addMovieJob, "queue", 5);
    }

    private async Task ProcessTvFolder(MediaFolder folder)
    {
        if (folder.Parsed is null) return;

        await using TmdbSearchClient tmdbSearchClient = new();
        var paginatedTvShowResponse = await tmdbSearchClient.TvShow(folder.Parsed.Title, folder.Parsed.Year);

        if (paginatedTvShowResponse?.Results.Length <= 0) return;

        // List<TvShow> res = Str.SortByMatchPercentage(paginatedTvShowResponse.Results, m => m.Name, folder.Parsed.Title);
        var res = paginatedTvShowResponse?.Results.ToList() ?? [];
        if (res.Count is 0) return;

        Titles.Add(res[0].Name);

        AddShowJob addShowJob = new(res[0].Id, Library);
        JobDispatcher.Dispatch(addShowJob, "queue", 5);
    }

    private async Task ScanAudioFolder(string path)
    {
        await using MediaScan mediaScan = new();
        IEnumerable<MediaFolder> rootFolders = (await mediaScan
                .Process(path, 2))
            .SelectMany(r => r.SubFolders ?? [])
            .ToList();
            // .Where(r => r.Path.Contains("Sensation 2001")).Where(r => r.Path.Contains("Daft Punk"))
            
        foreach (var rootFolder in rootFolders)
        {            
            if (rootFolder.Path == path) return;

            Titles.Add(rootFolder.Path);

            Logger.App($"Processing {rootFolder.Path}", LogLevel.Verbose);
        
            AddMusicJob addMusicJob = new(rootFolder.Path, Library);
            JobDispatcher.Dispatch(addMusicJob, "queue", 5);
        }

        Logger.App("Found " + Titles.Count + " subfolders");
    }

    ~LibraryLogic()
    {
        Dispose();
    }

    public void Dispose()
    {
        _mediaContext.Dispose();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    public async ValueTask DisposeAsync()
    {
        await _mediaContext.DisposeAsync();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}