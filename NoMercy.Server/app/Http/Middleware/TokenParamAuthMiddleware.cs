using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Middleware;

public class TokenParamAuthMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly MediaContext MediaContext = new();
    public static readonly List<User> Users = MediaContext.Users.ToList();
    public static readonly List<Ulid> FolderIds = MediaContext.Folders.Select(x => x.Id).ToList();

    public TokenParamAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string url = context.Request.Path;
        
        if (!FolderIds.Any(x => url.StartsWith("/" + x)) || context.Request.Headers.Authorization.ToString().Contains("Bearer"))
        {
            await _next(context);
            return;
        }
        
        string? claim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(string.IsNullOrEmpty(claim))
        {
            var jwt = context.Request.Query
                .FirstOrDefault(q => q.Key is "token" or "access_token").Value.ToString();
            
            if (string.IsNullOrEmpty(jwt))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;                
                return;
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
            
            User? user = Users.FirstOrDefault(x => x.Id == userId);
            
            if (user is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }
        
        await _next(context);
    }
}
