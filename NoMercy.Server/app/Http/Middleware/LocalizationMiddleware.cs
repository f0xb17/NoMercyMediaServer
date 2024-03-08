using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Http.Middleware;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = new RequestLocalizationOptions().RequestCultureProviders.FirstOrDefault();
        
        if (culture is not null)
        {
            var userLanguages = context.Request.Headers["Accept-Language"].ToString();
            var firstLang = userLanguages.Split(',').FirstOrDefault()?.Split('-');

            if (firstLang is not null && firstLang.Length > 0)
            {
                context.Request.Headers.AcceptLanguage = firstLang;
            }
        }
       
        await _next(context);
    }
}