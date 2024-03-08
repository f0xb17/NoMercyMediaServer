using System.Web.Http;

namespace NoMercy.Server.app.Http.Controllers.Socket;

public class SocketHub : ConnectionHub
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SocketHub(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

}