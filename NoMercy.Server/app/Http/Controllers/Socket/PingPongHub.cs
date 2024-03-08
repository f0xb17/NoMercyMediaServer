using System.Web.Http;
using Microsoft.AspNetCore.SignalR;

namespace NoMercy.Server.app.Http.Controllers.Socket;

public class PingPongHub : ConnectionHub
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PingPongHub(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SendPing()
    {
        await Clients.Caller.SendAsync("ReceivePong");
    }
    
}
