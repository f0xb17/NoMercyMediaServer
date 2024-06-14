using System.Security.Authentication;
using System.Security.Claims;

namespace NoMercy.Server.app.Http.Middleware;

public static class ClaimsPrincipleExtensions
{
    public static Guid UserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new AuthenticationException("User ID not found");
    }
}