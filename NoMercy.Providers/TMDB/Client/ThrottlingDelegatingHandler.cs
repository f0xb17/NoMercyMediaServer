namespace NoMercy.Providers.TMDB.Client;

public class ThrottlingDelegatingHandler : DelegatingHandler
{
    private SemaphoreSlim _throttler;

    public ThrottlingDelegatingHandler(SemaphoreSlim throttler)
    {
        _throttler = throttler ?? throw new ArgumentNullException(nameof(throttler));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        await _throttler.WaitAsync(cancellationToken);
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _throttler.Release();
        }
    }
}