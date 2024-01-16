using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace NoMercy.Providers.TMDB.Client;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class ImageClient : BaseClient
{
    public static Task<Bitmap> Download(string path, string size)
    {
        async Task<Bitmap> Task()
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://image.tmdb.org/t/p/");
            var response = await httpClient.GetAsync($"{size}{path}");
            var stream = await response.Content.ReadAsStreamAsync();
            return new Bitmap(stream);
        }

        return GetQueue().Enqueue(Task, $"{size}{path}");
    }
}