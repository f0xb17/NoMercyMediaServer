using System.Text.RegularExpressions;

namespace NoMercy.Server.MediaBrowser;

public class FileScanner
{
    public static string[] ScanFiles(string folder, string? ignoreRegex = null, string[]? filterExtensions = null, bool recursive = false)
    {
        filterExtensions ??= [".mp4", ".mkv", ".avi", ".ogv", ".m3u8", ".webm", ".vp9"];
        ignoreRegex ??= @"video_.*|audio_.*|subtitles|scans|cds.*|ost|album|music|original|fonts|thumbs|metadata|NCED|NCOP|\s\(\d\)\.|~";

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var allFiles = Directory.GetFiles(folder, "*.*", searchOption);

        var regex = new Regex(ignoreRegex, RegexOptions.IgnoreCase);
        var filteredFiles = allFiles
            .Where(f => !regex.IsMatch(f) && !filterExtensions.Contains(Path.GetExtension(f)))
            .ToArray();

        return filteredFiles;
    }
}