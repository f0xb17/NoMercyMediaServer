using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.TV;
using Episode = NoMercy.Providers.TMDB.Models.Episode.Episode;

namespace NoMercy.Server.Helpers;

public static class FileNameParsers
{
    private static string Pad(int number, int width)
    {
        return number.ToString().PadLeft(width, '0');
    }

    private static string _cleanFileName(string name)
    {
        name = Regex.Replace(name, "/", ".");
        name = Regex.Replace(name, ":\\s", ".");
        name = Regex.Replace(name, "\\s", ".");
        name = Regex.Replace(name, "\\? {2}", ".");
        name = Regex.Replace(name, "\\? ", ".");
        name = Regex.Replace(name, "u,\\.", ".");
        name = Regex.Replace(name, "u, ", ".");
        name = Regex.Replace(name, "`", "");
        name = Regex.Replace(name, "'", "");
        name = Regex.Replace(name, "’", "");
        name = Regex.Replace(name, "\"", "");
        name = Regex.Replace(name, "u,", ".");
        name = Regex.Replace(name, "\"", "'");
        name = Regex.Replace(name, "\\. {2,}", ".");
        name = Regex.Replace(name, "\\s", ".");
        name = Regex.Replace(name, "&", "and");
        name = Regex.Replace(name, "#", "%23");
        name = Regex.Replace(name, "!", "");
        name = Regex.Replace(name, "\\*", "-");
        name = Regex.Replace(name, "\\.\\.", ".");
        name = Regex.Replace(name, "u,\\.", ".");
        name = Regex.Replace(name, ": ", ".");
        name = Regex.Replace(name, ":", ".");
        name = Regex.Replace(name, "\\. *$", "");
        name = Regex.Replace(name, "'|\\?|\\.\\s|-\\.|\\.\\(\\d {1,3}\\)|[^[:print:]\\]|[^-_.[:alnum:]\\]", "");
        name = Regex.Replace(name, "\\. {2,}", ".");
        
        return name.Trim();
    }
    public static string CleanFileName<T>(this T self) where T: class
    {
        return _cleanFileName((self as string)!);
    }

    public static string CreateBaseFolder(TvShow show)
    {
        return string.Concat(show.Name.CleanFileName(), ".(",  show.FirstAirDate.ParseYear(), ")");
    }
    public static string CreateBaseFolder(MovieDetails movie)
    {
        return string.Concat(movie.Title.CleanFileName(), "(",  movie.ReleaseDate.ParseYear(), ")");
    }
    
    public static string CreateEpisodeFolder(Episode data, TvShow show)
    {
        return $"{show.Name}.S{Pad(data.SeasonNumber, 2)}E{Pad(data.EpisodeNumber, 2)}".CleanFileName();
    }
    
    public static string CreateTitleSort(string title, object? date = null)
    {
        // Step 1: Capitalize the first letter of the title
        title = char.ToUpper(title[0]) + title.Substring(1);

        // Step 2: Remove leading "The", "An", and "A" from the title
        title = Regex.Replace(title, "^The[\\s]*", "", RegexOptions.IgnoreCase);
        title = Regex.Replace(title, "^An[\\s]{1,}", "", RegexOptions.IgnoreCase);
        title = Regex.Replace(title, "^A[\\s]{1,}", "", RegexOptions.IgnoreCase);

        // Step 3: Replace ": " and " and the" with the parsed year (if available) or "."
        string replacement = date != null ? $".{date.ParseYear()}." : ".";
        title = Regex.Replace(title, ":\\s|\\sand\\sthe", replacement, RegexOptions.IgnoreCase);

        // Step 4: Replace all "." with " "
        title = title.Replace(".", " ");

        // Step 5: Convert the title to lowercase
        title = title.ToLower();
        
        return title.CleanFileName();

    }
    
    public static string CreateMediaFolder(Library library, MovieDetails movie)
    {
        string baseFolder = library.Folder_Libraries.First().Folder.Path;

        return $"{baseFolder}/{CreateBaseFolder(movie)}".CleanFileName();
    }
    public static string CreateMediaFolder(Library library, TvShow tv)
    {
        string baseFolder = library.Folder_Libraries.First().Folder.Path;

        return $"{baseFolder}/{CreateBaseFolder(tv)}".CleanFileName();
    }
    
    public static string CreateFileName(MovieDetails movie)
    {
        string name = $"{movie.Title}.({movie.ReleaseDate.ParseYear()}).NoMercy";
        return name.CleanFileName();
    }

    public static string CreateFileName(Episode episode, TvShow tvShow)
    {
        string name = $"{tvShow.Name}.S{Pad(episode.SeasonNumber, 2)}E{Pad(episode.EpisodeNumber, 2)}.{episode.Name}.NoMercy";
        return name.CleanFileName();
    }
    
    public static string? CreateRootFolderName(string folder)
    {
        using var context = new MediaContext();
        return context.Libraries
            .Include(l => l.Folder_Libraries)
            .SelectMany(l => l.Folder_Libraries)
            .FirstOrDefault(m => folder.Contains(m.Folder.Path))?.Folder.Path;
    }

}