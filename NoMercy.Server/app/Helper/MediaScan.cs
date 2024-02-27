using System.Text.RegularExpressions;
using FFMpegCore;
using FFMpegCore.Builders.MetaData;
using MovieFileLibrary;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Helpers;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Helper;

public class MovieFolder
{
    public string Title { get; set; }
    public string Year { get; set; }
    public string FilePath { get; set; }
}

public class MediaFolder
{
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<MediaFile>? Files { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<MediaFolder>? SubFolders { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public MovieFolder? Parsed { get; set; }
}

public class MediaFile
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Extension { get; set; }
    public int Size { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public MovieFile? Parsed { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IMediaAnalysis? FFprobe { get; set; }
    
}

public class MediaScan
{
    private readonly MediaContext _mediaContext = new();
    private readonly MovieDetector _movieDetector = new();
    
    private bool _fileListingEnabled;
    private bool _regexFilterEnabled = true;
    private readonly Regex _folderNameRegex = new(@"video_.*|audio_.*|subtitles|scans|cds.*|ost|album|music|original|fonts|thumbs|metadata|NCED|NCOP|\s\(\d\)\.|~", RegexOptions.IgnoreCase);

    public MediaScan()
    {
        GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(AppFiles.BinariesPath, "ffmpeg"));
    }
    
    public MediaScan EnableFileListing()
    {
        _fileListingEnabled = true;

        return this;
    }
    
    public MediaScan DisableRegexFilter()
    {
        _regexFilterEnabled = false;

        return this;
    }
    
    public Task<List<MediaFolder>?> Process(string rootFolder, int depth = 0)
    {
        return !_fileListingEnabled 
            ? Task.Run(() => ScanFoldersOnly(rootFolder, depth)) 
            : Task.Run(() => ScanFolder(rootFolder, depth));
    }

    private List<MediaFolder>? ScanFolder(string folderPath, int depth)
    {
        var folders = new List<MediaFolder>();
        if (depth < 0) return folders;
        
        List<MediaFile> files = GetFiles(folderPath);
        
        MovieFile movieFile1 = _movieDetector.GetInfo(folderPath);
        movieFile1.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})").Match(folderPath)
            .Value;
        
        folders.Add(new MediaFolder
        {
            Name = Path.GetFileName(folderPath),
            Path = folderPath,
            Created = Directory.GetCreationTime(folderPath),
            Modified = Directory.GetLastWriteTime(folderPath),
            Accessed = Directory.GetLastAccessTime(folderPath),
            Type = "folder",
            Parsed = new MovieFolder
            {
                Title = movieFile1.Title ?? "",
                Year = movieFile1.Year,
                FilePath = movieFile1.FilePath,
            },
            
            Files = files.Count > 0
                ? files
                : null,
        });
        
        var directories = Directory.GetDirectories(folderPath).OrderBy(f => f);
        
        foreach (var directory in directories)
        {
            var folderName = Path.GetFileName(directory);
            
            if (_regexFilterEnabled && _folderNameRegex.IsMatch(folderName) || depth == 0)
            {
                files.Add(new MediaFile
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",
                });
                
                continue;
            }
            
            List<MediaFile> files2 = depth - 1 > 0 ? GetFiles(folderPath) : [];

            MovieFile movieFile = _movieDetector.GetInfo(directory);
            movieFile.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})").Match(directory).Value;
            
            folders.Add(new MediaFolder
            {
                Name = folderName,
                Path = directory,
                Created = Directory.GetCreationTime(directory),
                Modified = Directory.GetLastWriteTime(directory),
                Accessed = Directory.GetLastAccessTime(directory),
                Type = "folder",
                Parsed = new MovieFolder
                {
                    Title = movieFile.Title ?? "",
                    Year = movieFile.Year,
                    FilePath = movieFile.FilePath,
                },
                
                Files = files2.Count > 0
                    ? files2 
                    : null,
                
                SubFolders = depth - 1 > 0 
                    ? ScanFolder(directory, depth - 1) 
                    : null,
            });
        }
        
        return folders
            .Where(f => f.Name is not "")
            .ToList();
    }

    private List<MediaFolder>? ScanFoldersOnly(string folderPath, int depth)
    {
        var folders = new List<MediaFolder>();
        if (depth < 0) return folders;

        var directories = Directory.GetDirectories(folderPath).OrderBy(f => f);
        
        foreach (var directory in directories)
        {
            var folderName = Path.GetFileName(directory);
            
            if (_regexFilterEnabled && _folderNameRegex.IsMatch(folderName))
            {
                folders.Add(new MediaFolder
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",
                });
                
                continue;
            }
            
            MovieFile movieFile = _movieDetector.GetInfo(directory);
            if (movieFile.Year is null)
            {
                movieFile.Year = new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})").Match(directory).Value;
            }
            
            folders.Add(new MediaFolder
            {
                Name = folderName,
                Path = directory,
                Created = Directory.GetCreationTime(directory),
                Modified = Directory.GetLastWriteTime(directory),
                Accessed = Directory.GetLastAccessTime(directory),
                Type = "folder",
                    
                Parsed = new MovieFolder
                {
                    Title = movieFile.Title ?? "",
                    Year = movieFile.Year,
                    FilePath = movieFile.FilePath,
                },
                
                SubFolders = depth - 1 > 0
                    ? ScanFoldersOnly(directory, depth - 1)
                    : null,
            });
        }
        
        return folders
            .Where(f => f.Name is not "")
            .ToList();
    }
    
    private List<MediaFile> GetFiles(string folderPath)
    {
        var files = new List<MediaFile>();
        foreach (var file in Directory.GetFiles(folderPath))
        {
            var extension = Path.GetExtension(file).ToLower();
            
            var isVideoFile = extension == ".mp4" 
                              || extension == ".avi" 
                              || extension == ".mkv" 
                              || extension == ".m3u8";
            
            MovieFile? movieFile = isVideoFile ? _movieDetector.GetInfo(file) : null;
            AnimeInfo? animeInfo = AnimeParser.ParseAnimeFilename(file);
            
            if (movieFile is not null)
            {
                movieFile.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})").Match(file).Value;
                movieFile.Title ??= animeInfo?.Name;
                movieFile.Season ??= animeInfo?.Season;
                movieFile.Episode ??= animeInfo?.Episode;
            }

            IMediaAnalysis? ffprobe = null;
            try
            {
                ffprobe = isVideoFile
                    ? FFProbe.Analyse(file)
                    : null;
            }
            catch (Exception e)
            {
               //
            }

            files.Add(new MediaFile
            {
                Name = Path.GetFileName(file),
                Path = file,
                Extension = extension,
                Size = (int)new FileInfo(file).Length,
                Created = File.GetCreationTime(file),
                Modified = File.GetLastWriteTime(file),
                Accessed = File.GetLastAccessTime(file),
                Type = "file",
                
                Parsed = movieFile,
                FFprobe = ffprobe
            });
        }
        
        return files
            .Where(f => f.Name is not "")
            .ToList();
    }

     public void Dispose()
     {
         GC.Collect();
         GC.WaitForFullGCComplete();
     }
}