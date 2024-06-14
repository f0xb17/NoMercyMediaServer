using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using NoMercy.Helpers;

namespace NoMercy.Providers.TMDB.Client;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public abstract class TmdbImageClient : TmdbBaseClient
{
    public static Task<Bitmap?> Download(string? path, bool? download = true)
    {
        return Queue().Enqueue(Task, $"original{path}", true);

        async Task<Bitmap?> Task()
        {
            if (path is null) return null;
            
            try
            {
                var folder = Path.Join(AppFiles.ImagesPath, "original");
                
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var filePath = Path.Join(folder, path.Replace("/", ""));

                if (File.Exists(filePath)) return new Bitmap(filePath);

                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("User-Agent", ApiInfo.UserAgent);
                httpClient.BaseAddress = new Uri("https://image.tmdb.org/t/p/");
                httpClient.DefaultRequestHeaders.Add("Accept", "image/*");
                httpClient.Timeout = TimeSpan.FromMinutes(5);

                var url = path.StartsWith("http") ? path : $"original{path}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;
                
                if (download is false) 
                    return new Bitmap(await response.Content.ReadAsStreamAsync());

                if (!File.Exists(filePath))
                    await File.WriteAllBytesAsync(filePath, await response.Content.ReadAsByteArrayAsync());
                
                return new Bitmap(await response.Content.ReadAsStreamAsync());
            }
            catch (Exception e)
            {
                Logger.MovieDb($"Error downloading image: {path} - {e.Message}", LogLevel.Error);
            }

            return null;
        }
    }
}