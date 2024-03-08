using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Http.Middleware;

public class TokenParamAuthMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly MediaContext MediaContext = new();
    private readonly List<User> _users = MediaContext.Users.ToList();

    public TokenParamAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        MediaContext mediaContext = new();
        List<Ulid> exemptions = mediaContext.Folders
            .Select(x => x.Id)
            .ToList();
            
        string url = context.Request.Path;
        
        if (!exemptions.Any(x => url.StartsWith("/" + x)))
        {
            await _next(context);
            return;
        }
        
        string? claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(claim is null)
        {
            var jwt = context.Request.Query
                .FirstOrDefault(q => q.Key == "token" || q.Key == "access_token").Value.ToString();
            
            if (string.IsNullOrEmpty(jwt))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            
            context.Request.Headers.Authorization = new StringValues("Bearer " + jwt);
        
        }
        else
        {
            Guid userId = Guid.Parse(claim);
            
            if (userId == Guid.Empty)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            
            User? user = _users.FirstOrDefault(x => x.Id == userId);
            
            if (user is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }
        
        await _next(context);
    }
}
