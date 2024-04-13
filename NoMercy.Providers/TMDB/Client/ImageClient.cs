using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using NoMercy.Helpers;

namespace NoMercy.Providers.TMDB.Client;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public abstract class ImageClient : BaseClient
{
    public static Task<Bitmap?> Download(string path, string size, bool? download = true)
    {
        return GetQueue().Enqueue(Task, $"{size}{path}", true);
        
        async Task<Bitmap?> Task()
        {
            try
            {
                using HttpClient httpClient = new();
                httpClient.BaseAddress = new Uri("https://image.tmdb.org/t/p/");
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                
                HttpResponseMessage response = await httpClient.GetAsync($"{size}{path}");
                Stream stream = await response.Content.ReadAsStreamAsync();

                if (download is false)
                {
                    return new Bitmap(stream);
                }
                
                string folder = Path.Join(AppFiles.ImagesPath, size);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                    
                string filePath = Path.Join(folder, path.Replace("/", ""));
                if (!File.Exists(filePath))
                {
                    await File.WriteAllBytesAsync(filePath, await response.Content.ReadAsByteArrayAsync());
                }
                return new Bitmap(stream);
            }
            catch (Exception e)
            {
                Logger.MovieDb($"Error downloading image: {path} - {e.Message}", LogLevel.Error);
            }

            return null;
        }
    }

    // public static Task<Bitmap?> Download(string path, string? destination = null)
    // {
    //     return GetQueue().Enqueue(Task, path);
    //
    //     async Task<Bitmap?> Task()
    //     {
    //         using var httpClient = new HttpClient();
    //         httpClient.BaseAddress = new Uri("https://assets.fanart.tv");
    //         
    //         if (path.StartsWith($"/"))
    //         {
    //             path = Path.Combine(AppFiles.CachePath, path);
    //             
    //             if (File.Exists(path)) return new Bitmap(path);
    //         }
    //         
    //         var response = await httpClient.GetAsync(path);
    //         var stream = await response.Content.ReadAsStreamAsync();
    //         
    //         if (destination is null)
    //         {
    //             return new Bitmap(stream);
    //         }
    //         
    //         string folder = Path.Join(AppFiles.ImagesPath, destination);
    //         if (!Directory.Exists(folder))
    //         {
    //             Directory.CreateDirectory(folder);
    //         }
    //
    //         Match match = Regex.Match(path, @"([\w]{8}-[\w]{4}-[\w]{4}-[\w]{4}-[\w]{12})");
    //         if(match.Success)
    //         {
    //             path = match.Groups[1].Value + ".jpg";
    //         }
    //
    //         string filePath = Path.Join(folder, path);
    //         if (!File.Exists(filePath))
    //         {
    //             await File.WriteAllBytesAsync(filePath, await response.Content.ReadAsByteArrayAsync());
    //         }
    //         
    //         return new Bitmap(stream);
    //     }
    // }
}