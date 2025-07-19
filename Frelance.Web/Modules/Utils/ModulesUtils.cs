using Frelance.Contracts.Enums;
using System.Security.Claims;

namespace Frelance.Web.Modules.Utils;

public static class ModulesUtils
{
    public static Role GetRole(this HttpContext httpContext)
    {
        var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        var commandRole = role == "Client" ? Role.Client : Role.Freelancer;
        return commandRole;
    }
}