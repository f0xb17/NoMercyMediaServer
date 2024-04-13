using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;
using Movie = NoMercy.Providers.TMDB.Models.Movies.Movie;

namespace NoMercy.Server.Logic;

public class LibraryLogic(string id)
{
    private readonly MediaContext _mediaContext = new();
    private Library Library { get; set; } = new();
    
    public Ulid Id { get; set; }
    
    private int Depth { get; set; }
    
    public List<dynamic> Titles { get; } = [];
    private List<string> Paths { get; } = [];
    // private List<MediaFolder> Folders { get; } = [];

    public async Task<bool> Process()
    {
        Id = Ulid.Parse(id);
        
        Library? library = await _mediaContext.Libraries
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
        foreach (string path in Paths)
        {
            switch (Library?.Type)
            {
                case "music":
                    await ScanAudioFolder(path);
                    break;
                default:
                    await ScanVideoFolder(path);
                    break;
            }
        }
        
        Logger.App("Scanning done");
    }

    private async Task ScanVideoFolder(string path)
    {
        MediaScan mediaScan = new();
        ConcurrentBag<MediaFolder> list = await mediaScan
            .Process(path, Depth);
                
        mediaScan.Dispose();
        
        foreach (MediaFolder folder in list)
        {
            await ProcessVideoFolder(folder);
        }
                
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
        if(folder.Parsed is null) return;
        
        SearchClient searchClient = new();
        PaginatedResponse<Movie>? paginatedMovieResponse = await searchClient.Movie(folder.Parsed.Title, folder.Parsed.Year);
        
        if (paginatedMovieResponse?.Results.Length <= 0) return;
        
        // List<Movie> res = Str.SortByMatchPercentage(paginatedMovieResponse?.Results, m => m.Title, folder.Parsed.Title);
        List<Movie> res = paginatedMovieResponse?.Results.ToList() ?? [];
        if (res.Count is 0) return;
                
        Titles.Add(res[0].Title);
        
        AddMovieJob addMovieJob = new AddMovieJob(id: res[0].Id, libraryId:Library.Id.ToString());
        JobDispatcher.Dispatch(addMovieJob, "queue", 5);
    }
    
    private async Task ProcessTvFolder(MediaFolder folder)
    {
        if(folder.Parsed is null) return;
        
        SearchClient searchClient = new();
        PaginatedResponse<TvShow>? paginatedTvShowResponse = await searchClient.TvShow(folder.Parsed.Title, folder.Parsed.Year);
        
        if (paginatedTvShowResponse?.Results.Length <= 0) return;
                
        // List<TvShow> res = Str.SortByMatchPercentage(paginatedTvShowResponse.Results, m => m.Name, folder.Parsed.Title);
        List<TvShow> res = paginatedTvShowResponse?.Results.ToList() ?? [];
        if (res.Count is 0) return;
                
        Titles.Add(res[0].Name);
        
        AddShowJob addShowJob = new AddShowJob(id: res[0].Id, libraryId: Library.Id.ToString());
        JobDispatcher.Dispatch(addShowJob, "queue", 5);
    }

    private async Task ScanAudioFolder(string path)
    {
        MediaScan mediaScan = new();
        IEnumerable<MediaFolder> rootFolders = (await mediaScan
            .Process(path, 2))
            // .Where(r => 
                // !r.Path.Contains("[Various Artists]") && 
                // // !r.Path.Contains("[Soundtracks]") &&
                // !r.Path.Contains("[Other]"))
            .SelectMany(r => r.SubFolders ?? [])
            // .Where(r => r.Path.Contains("Heartbound"))
            .ToList();
                
        mediaScan.Dispose();

        foreach (MediaFolder? rootFolder in rootFolders)
        {
            if (rootFolder.Path == path) return;

            Titles.Add(rootFolder.Path);
            
            await ProcessAudioFolder(rootFolder);
        };
        
        Logger.App("Found " + Titles.Count + " subfolders");
    }

    private async Task ProcessAudioFolder(MediaFolder folder)
    {
        Logger.App($"Processing {folder.Path}", LogLevel.Verbose);
        
        MediaScan mediaScan = new();
        IEnumerable<MediaFolder> list = (await mediaScan
                .EnableFileListing()
                .Process(folder.Path, 20))
                .OrderBy(r => r.Path);
                // .Where(r => r.Path.Contains("30"));
                
        mediaScan.Dispose();

        foreach (MediaFolder mediaFolder in list)
        {
            Logger.App($"Processing {mediaFolder.Path} and has {mediaFolder.Files?.Count ?? 0} files", LogLevel.Verbose);

            foreach (MediaFile file in mediaFolder.Files ?? [])
            {
                if (file.Parsed is null) continue;

                Titles.Add(file.Path);

                AddMusicJob addMusicJob = new AddMusicJob(libraryId: Library.Id.ToString(), file: file.Path);
                JobDispatcher.Dispatch(addMusicJob, "queue", 5);
            }
        };
    }
}