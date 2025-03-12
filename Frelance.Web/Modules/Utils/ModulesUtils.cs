using System.Security.Claims;
using Frelance.Contracts.Enums;

namespace Frelance.Web.Modules.Utils;

public static class ModulesUtils
{
    public static Role GetRole(HttpContext httpContext)
    {
        var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        var commandRole = role == "Client" ? Role.Client : Role.Freelancer;
        return commandRole;
    }
}