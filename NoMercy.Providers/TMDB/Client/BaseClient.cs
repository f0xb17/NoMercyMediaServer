using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using NoMercy.Providers.Helpers;
using NoMercy.Providers.TMDB.Models.Shared;
using Microsoft.Extensions.Caching.Memory;
using NoMercy.Providers.Properties;

namespace NoMercy.Providers.TMDB.Client
{
    public class BaseClient
    {
        private readonly Uri _baseUrl = new("https://api.themoviedb.org/3/");

        private static readonly FixedWindowRateLimiter RateLimiter = new(new FixedWindowRateLimiterOptions()
        {
            Window = TimeSpan.FromSeconds(1),
            PermitLimit = 50,
            QueueLimit = int.MaxValue,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            AutoReplenishment = true,
        });
        
        // private readonly HttpClient _client = new(new ThrottlingDelegatingHandler(new SemaphoreSlim(50)));
        // private readonly HttpClient _client = new(new ThrottlingDelegatingHandler(new SemaphoreSlim(50)));
        // private readonly HttpClient _client = new(new ClientSideRateLimitedHandler(RateLimiter));
        private readonly HttpClient _client = new();
        
        private const int CacheDurationInHours = 1;

        private readonly Dictionary<string, string> _defaultQuery = new();
        public Uri? Uri;

        public BaseClient()
        {
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected BaseClient(int id)
        {
            _client = new HttpClient
            {
                BaseAddress = _baseUrl
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Id = id;
        }

        public BaseClient(Uri uri)
        {
            Uri = uri;
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public BaseClient(HttpClient client, Uri uri)
        {
            _client = client;
            _client.BaseAddress = _baseUrl;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Uri = uri;
        }

        private static string ApiKey => Resources.TmdbApiKey;

        private static Queue? _queue;

        private static Queue GetQueue()
        {
            return _queue ??= new Queue(new QueueOptions { Concurrent = 50, Interval = 1000, Start = true });
        }
        
        private static IMemoryCache? _memoryCache;
            
        private static IMemoryCache GetMemoryCache()
        {
            return _memoryCache ??= new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024*1024*1024
            });
        }

        private static int Max(int available, int wanted, int constraint) {
            return wanted < available
                ? wanted > constraint
                    ? constraint
                    : wanted
                : available;
        }

        public int Id { get; private set; }

        protected async Task<T> Get<T>(string url, Dictionary<string, string>? query = null) where T : class
        {
            query ??= _defaultQuery;

            query["api_key"] = Resources.TmdbApiKey ?? "ed3bf860adef0537783e4abee86d65af";

            var newUrl = QueryHelpers.AddQueryString(url, query!);

            if (GetMemoryCache().TryGetValue(newUrl, out T? result))
            {
                return result!;
            }

            Console.WriteLine(@"{0}: {1}", DateTime.Now, newUrl);

            var response = await GetQueue().Enqueue(() => _client.GetStringAsync(newUrl), newUrl);
            // var response = await _client.GetStringAsync(newUrl);

            result = JsonHelper.FromJson<T>(response);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(CacheDurationInHours))
                .SetSize(result.ToString()?.Length ?? 0);

            GetMemoryCache().Set(newUrl, result, cacheEntryOptions);
            
            return result;
        }

        protected async Task<List<T>> Paginated<T>(string url, int limit) where T: class
        {
            List<T> list = new List<T>();
        
            var firstPage = await Get<PaginatedResponse<T>>(url);
            list.AddRange(firstPage.Results);

            if (limit > 1)
            {
                await Parallel.ForAsync(2, Max(firstPage.TotalPages, limit, 500), async (i, state) =>
                {
                    var page = await Get<PaginatedResponse<T>>(url, new Dictionary<string, string>
                    {
                        ["page"] = i.ToString()
                    });
                    
                    lock(list)
                        list.AddRange(page.Results);
                });
            }

            return list;
        }
    }
}

internal sealed class ClientSideRateLimitedHandler(
    RateLimiter limiter)
    : DelegatingHandler(new HttpClientHandler()), IAsyncDisposable
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using RateLimitLease lease = await limiter.AcquireAsync(
            permitCount: 1, cancellationToken);

        if (lease.IsAcquired)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        if (lease.TryGetMetadata(
                MetadataName.RetryAfter, out TimeSpan retryAfter))
        {
            response.Headers.Add(
                "Retry-After",
                ((int)retryAfter.TotalSeconds).ToString(
                    NumberFormatInfo.InvariantInfo));
        }

        return response;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    { 
        await limiter.DisposeAsync().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            limiter.Dispose();
        }
    }
}