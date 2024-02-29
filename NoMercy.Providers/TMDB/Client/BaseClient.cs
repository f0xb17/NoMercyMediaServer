using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using NoMercy.Providers.Helpers;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Helpers;

namespace NoMercy.Providers.TMDB.Client
{
    public class BaseClient : IDisposable
    {
        private readonly Uri _baseUrl = new("https://api.themoviedb.org/3/");

        private readonly HttpClient _client = new();

        protected BaseClient()
        {
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiInfo.TmdbToken}");
        }

        protected BaseClient(int id)
        {
            _client = new HttpClient
            {
                BaseAddress = _baseUrl
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiInfo.TmdbToken}");
            Id = id;
        }

        private static Queue? _queue;

        protected static Queue GetQueue()
        {
            return _queue ??= new Queue(new QueueOptions { Concurrent = 50, Interval = 1000, Start = true });
        }

        private static int Max(int available, int wanted, int constraint)
        {
            return wanted < available
                ? wanted > constraint
                    ? constraint
                    : wanted
                : available;
        }

        public int Id { get; private set; }

        protected async Task<T> Get<T>(string url, Dictionary<string, string> query = null) where T : class
        {
            query ??= new Dictionary<string, string>();

            var newUrl = QueryHelpers.AddQueryString(url, query);

            if (CacheController.Read(newUrl, out T? result))
            {
                return result;
            }

            Logger.MovieDb(newUrl);

            var response = await GetQueue().Enqueue(() => _client.GetStringAsync(newUrl), newUrl);

            await CacheController.Write(newUrl, response);

            var data = JsonHelper.FromJson<T>(response);

            return data;
        }

        protected async Task<List<T>> Paginated<T>(string url, int limit) where T : class
        {
            List<T> list = new List<T>();

            var firstPage = await Get<PaginatedResponse<T>>(url);
            list.AddRange(firstPage?.Results ?? []);

            if (limit > 1)
            {
                await Parallel.ForAsync(2, Max(firstPage?.TotalPages ?? 0, limit, 500), async (i, _) =>
                {
                    var page = await Get<PaginatedResponse<T>>(url, new Dictionary<string, string>
                    {
                        ["page"] = i.ToString()
                    });

                    lock (list)
                    {
                        list.AddRange(page?.Results ?? []);
                    }
                });
            }

            return list;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
