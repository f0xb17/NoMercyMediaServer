using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Http.Middleware;

public class AccessLogMiddleware
{
    
    private readonly RequestDelegate _next;

    public AccessLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? guid = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (guid is null)
        {
            Logger.Http($"Unknown: {context.Request.Path}");
            await _next(context);
            return;
        }
        
        Guid userId = Guid.Parse(guid);
        if(userId == Guid.Empty)
        {
            Logger.Http($"Unknown: {context.Request.Path}");
            await _next(context);
            return;
        }
        
        await using MediaContext mediaContext = new();
        User? user = await mediaContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
        
        Logger.Http($"{user?.Name ?? "Unknown"}: {context.Request.Path}");
        
        await _next(context);
    }
}