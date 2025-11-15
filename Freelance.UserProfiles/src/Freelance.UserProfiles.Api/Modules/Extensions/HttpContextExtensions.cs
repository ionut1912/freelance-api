using System.Security.Claims;

namespace Freelance.UserProfiles.Api.Modules.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetAccountId(this HttpContext httpContext)
    {
        var accountId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(accountId) || !Guid.TryParse(accountId, out var guid))
            return Guid.Empty;
        return guid;
    }
}