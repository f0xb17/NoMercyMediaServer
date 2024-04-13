using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using NoMercy.Helpers;
using NoMercy.Providers.AcoustId.Models;
using NoMercy.Providers.Helpers;

namespace NoMercy.Providers.AcoustId.Client
{
    public class BaseClient : IDisposable
    {
        private readonly Uri _baseUrl = new("https://api.acoustid.org/v2/");

        private readonly HttpClient _client = new();

        protected BaseClient()
        {
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent","NoMercy MediaServer");
        }

        protected BaseClient(Guid id)
        {
            _client = new HttpClient
            {
                BaseAddress = _baseUrl
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent","NoMercy MediaServer");
            Id = id;
        }

        private static Queue? _queue;

        private static Queue GetQueue()
        {
            return _queue ??= new Queue(new QueueOptions { Concurrent = 3, Interval = 1000, Start = true });
        }

        protected Guid Id { get; private set; }
        
        protected async Task<T?> Get<T>(string url, Dictionary<string, string>? query = null, bool? priority = false) where T : class
        {
            query ??= new Dictionary<string, string>();

            var newUrl = QueryHelpers.AddQueryString(url, query!);
            
            if (CacheController.Read(newUrl, out Fingerprint? result))
            {
                if (result?.Results.Length > 0 && result.Results
                        .Any(fpResult => fpResult.Recordings
                            .Any(recording => recording.Title != null))) return result as T;
            }

            Logger.AcoustId(newUrl, LogLevel.Verbose);

            var response = await GetQueue().Enqueue(() => _client.GetStringAsync(newUrl), newUrl, priority);

            await CacheController.Write(newUrl, response);
            
            var data = JsonHelper.FromJson<Fingerprint>(response);
            
            int iteration = 0;

            if (data?.Results.Length > 0 && data.Results
                    .Any(fpResult => fpResult.Recordings
                        .Any(recording => recording.Title != null))) return data as T;

            while (data?.Results.Length == 0 && data.Results
                       .Any(fpResult => fpResult.Recordings
                           .Any(recording => recording.Title == null)) && iteration < 10)
            {
                response = await GetQueue().Enqueue(() => _client.GetStringAsync(newUrl), newUrl, priority);

                await CacheController.Write(newUrl, response);
                
                Logger.Request(response, LogLevel.Verbose);

                data = JsonHelper.FromJson<Fingerprint>(response);
                
                iteration++;
            }
            
            return data as T;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
