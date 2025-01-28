using System.Security.Claims;
using Frelance.Application.Repositories;
using Microsoft.AspNetCore.Http;

namespace Frelance.Infrastructure.Services;

public class UserAccessor:IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor,nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetUsername()
    {
        var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        return username;
    }

}