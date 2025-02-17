using System.Security.Claims;
using Frelance.Application.Repositories;
using Frelance.Contracts.Exceptions;
using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace Frelance.Infrastructure.Services;

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
        return username?? throw new NotFoundException($"{nameof(Users)} with {nameof(Users.UserName)}: {username} not found");
    }

}