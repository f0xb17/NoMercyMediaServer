using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MovieFileLibrary;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;

namespace NoMercy.Server.Logic;

public class FileLogic
{
    private readonly MediaContext _mediaContext = new();
    private readonly MovieDetector _movieDetector = new();

    private int Id { get; set; }
    private Library Library { get; set; }
    private Movie? Movie { get; set; }
    private Tv? Show { get; set; }

    private List<Folder> Folders { get; set; } = [];
    public List<MediaFolder> Files { get; set; } = [];
    public string Type { get; set; } = "";

    public FileLogic(int id, Library library)
    {
        Id = id;
        Library = library;
    }

    public async Task Process()
    {
        await GetType();
        GetPaths();

        foreach (var folder in Folders)
        {
            List<MediaFolder> files = await GetFiles(folder.Path);

            if (files.Count > 0)
            {
                Files.AddRange(files);
            }
        }

        switch (Library.Type)
        {
            case "movie":
                await StoreMovie();
                break;
            case "tv":
                await StoreTvShow();
                break;
            default:
                Logger.App("Unknown library type");
                break;
        }
    }

    private async Task StoreMovie()
    {
        await using MediaContext context = new MediaContext();

        MediaFile? item = Files.FirstOrDefault(file => file.Parsed is not null)
            ?.Files?.FirstOrDefault(file => file.Parsed is not null);
        
        if (item == null) return;

        await StoreItem(item);
    }

    private async Task StoreTvShow()
    {
        await using MediaContext context = new MediaContext();

        var items = Files
            .Where(mediaFolder => mediaFolder.SubFolders?.Any() ?? false)
            .Select(mediaFolder => mediaFolder.SubFolders?.FirstOrDefault())
            .Where(mediaFolder => mediaFolder?.Parsed is not null)
            .Select(mediaFolder => mediaFolder?.Files?.Where(mediaFile => mediaFile.Parsed is not null))
            .Select(mediaFiles => mediaFiles?.FirstOrDefault())?
            .ToList() ?? [];
        
        if (items.Count == 0) return;

        foreach (var item in items)
        {
            await StoreItem(item);
        }
    }

    private async Task StoreItem(MediaFile? item)
    {
        if (item?.Parsed is null) return;

        var folder = Folders.FirstOrDefault(folder => item.Path.Contains(folder.Path));
        if (folder == null) return;

        List<Subtitle> subtitles = [];
        
        string fileName = Path.DirectorySeparatorChar + Path.GetFileName(item.Path);
        string hostFolder = item.Path.Replace(fileName, "");
        string baseFolder = Path.DirectorySeparatorChar + (Movie?.Folder ?? Show?.Folder ?? "")
                                                        + item.Path.Replace(folder.Path, "")
                                                            .Replace(fileName, "");

        string subtitleFolder = Path.Combine(hostFolder, "subtitles");
        
        if (Directory.Exists(subtitleFolder))
        {
            var subtitleFiles = Directory.GetFiles(subtitleFolder);
            foreach (var subtitleFile in subtitleFiles)
            {
                Regex regex = new Regex(@"(?<lang>\w{3}).(?<type>\w{3,4}).(?<ext>\w{3})$");
                Match match = regex.Match(subtitleFile);

                subtitles.Add(new Subtitle
                {
                    Language = match.Groups["lang"].Value,
                    Type = match.Groups["type"].Value,
                    Ext = match.Groups["ext"].Value,
                });
            }
        }
        
        Episode? episode = await _mediaContext.Episodes
            .Where(e => Show != null && e.TvId == Show.Id)
            .Where(e => e.SeasonNumber == item.Parsed.Season)
            .Where(e => e.EpisodeNumber == item.Parsed.Episode)
            .FirstOrDefaultAsync(); 
        
        VideoFile videoFile = new VideoFile
        {
            EpisodeId = episode?.Id,
            MovieId = Movie?.Id,
            Folder = baseFolder.Replace("\\", "/"),
            HostFolder = hostFolder.Replace("\\", "/"),
            Filename = fileName.Replace("\\", "/"),

            Share = folder.Id.ToString() ?? "",
            Duration = Regex.Replace(
                Regex.Replace(item.FFprobe?.Duration.ToString() ?? "", "\\.\\d+" , "")
                , "^00:", ""),
            // Chapters = JsonConvert.SerializeObject(item.FFprobe?.Chapters ?? []),
            Chapters = "",
            Languages = JsonConvert.SerializeObject(item.FFprobe?.AudioStreams.Select(stream => stream.Language)),
            Quality = item.FFprobe?.VideoStreams.FirstOrDefault()?.Width.ToString() ?? "",
            Subtitles = JsonConvert.SerializeObject(subtitles),
        };

        await _mediaContext.VideoFiles.Upsert(videoFile)
            .On(vf => vf.Filename)
            .WhenMatched((vs, vi) => new VideoFile
            {
                Id = vi.Id,
                EpisodeId = vi.EpisodeId,
                MovieId = vi.MovieId,
                Folder = vi.Folder,
                HostFolder = vi.HostFolder,
                Filename = vi.Filename,
                Share = vi.Share,
                Duration = vi.Duration,
                Chapters = vi.Chapters,
                Languages = vi.Languages,
                Quality = vi.Quality,
                Subtitles = vi.Subtitles,
            })
            .RunAsync();
    }

    private async Task GetType()
    {
        switch (Library.Type)
        {
            case "movie":
                Movie = await _mediaContext.Movies
                    .Where(m => m.Id == Id)
                    .FirstOrDefaultAsync();
                Type = "movie";
                break;
            case "tv":
                Show = await _mediaContext.Tvs
                    .Where(t => t.Id == Id)
                    .FirstOrDefaultAsync();
                Type = "tv";
                break;
        }
    }

    private async Task<List<MediaFolder>> GetFiles(string path)
    {
        Logger.App($@"Files: {path}");
        MediaScan mediaScan = new();

        int depth = Library.Type switch
        {
            "movie" => 1,
            "tv" => 2,
            _ => 1
        };

        List<MediaFolder>? folders = await mediaScan
            .EnableFileListing()
            .Process(path, depth);

        mediaScan.Dispose();

        return folders ?? [];
    }

    private void GetPaths()
    {
        string? folder = Library.Type switch
        {
            "movie" => Movie?.Folder,
            "tv" => Show?.Folder,
            _ => ""
        };

        if (folder == null) return;

        var rootFolders = Library.FolderLibraries
            .Select(f => f.Folder)
            .ToArray();

        foreach (var rootFolder in rootFolders)
        {
            string path = Path.Combine(rootFolder.Path, folder);

            if (Directory.Exists(path))
            {
                Folders.Add(new Folder
                {
                    Path = path,
                    Id = rootFolder.Id,
                });
            }
        }
    }

    public void Dispose()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}

public class Subtitle
{
    [JsonProperty("language")] public string Language { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("ext")] public string Ext { get; set; }
}