using System.Net;
using System.Security.Claims;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Middleware;

public class HasAccessMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly MediaContext MediaContext = new();
    private readonly List<User> _users = MediaContext.Users.ToList();

    public HasAccessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (claim is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        Guid userId = Guid.Parse(claim);
        
        if (userId == Guid.Empty)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        User? user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        
        await _next(context);
    }
}