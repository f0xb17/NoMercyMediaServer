using System.Text;
using NoMercy.Helpers;

namespace NoMercy.Providers.Helpers;

public static class CacheController
{
    public static string GenerateFileName(string url)
    {
        return CreateMd5(url);
    }

    private static string CreateMd5(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    public static bool Read<T>(string url, out T? value) where T : class?
    {
        // Console.WriteLine(@"Reading from cache for {0} path {1}", url, GenerateFileName(url));

        string fullname = Path.Combine(AppFiles.ApiCachePath, GenerateFileName(url));
        lock (fullname)
        {
            if(File.Exists(fullname) == false)
            {
                value = default;
                return false;
            }

            T? data;
            try
            {
                string d = File.ReadAllTextAsync(fullname).Result;
                data = JsonHelper.FromJson<T>(d);
                
            }
            catch (Exception _)
            {
                value = default;
                return false;
            }
            
            if (data == null)
            {
                value = default;
                return true;
            }
            
            if (data is { } item)
            {
                value = item;
                return true;
            }
            
            value = default;
            return false;
        }
    }
    
    public static async Task Write(string url, string data)
    {
        string fullname = Path.Combine(AppFiles.ApiCachePath, GenerateFileName(url));

        try
        {
            await File.WriteAllTextAsync(fullname, data);
        }
        catch (Exception _)
        {
            await Write(url, data);
        }
        
    }
}