using System.Security.Claims;
using Freelance.Application.Repositories;
using Freelance.Contracts.Exceptions;
using Freelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace Freelance.Infrastructure.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        var username = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Name)?.Value;
        return username ??
               throw new NotFoundException($"{nameof(Users)} with {nameof(Users.UserName)}: {username} not found");
    }
}