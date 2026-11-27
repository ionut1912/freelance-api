using System.Security.Claims;
using System.Text;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Infra.Settings;

namespace Freelance.Identity.Infrastructure.Persistance.Repositories;

/// <summary>
/// Issues HMAC-SHA256 signed JWTs using <see cref="JsonWebTokenHandler"/> (the modern, faster replacement
/// for <c>JwtSecurityTokenHandler</c>).
/// </summary>
public sealed class JwtTokenService(IOptions<JwtSettings> options, TimeProvider timeProvider) : IJwtTokenService
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(2);
    private static readonly JsonWebTokenHandler TokenHandler = new();

    public string GenerateToken(Account account)
    {
        ArgumentNullException.ThrowIfNull(account);

        var settings = options.Value;
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.Add(TokenLifetime),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = account.Id.ToString(),
                [ClaimTypes.Name] = account.Username,
                [JwtRegisteredClaimNames.Email] = account.Email,
                [ClaimTypes.Role] = account.Role.Value,
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString()
            }
        };

        return TokenHandler.CreateToken(descriptor);
    }
}
