using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Frelance.Infrastructure.Entities;
using Frelance.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Frelance.Infrastructure.Services;

public class TokenService
{
    private readonly UserManager<Users> _userManager;
    private readonly string _jwtSecretKey;

    public TokenService(IConfiguration configuration, UserManager<Users> userManager)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        _userManager = userManager;

        var jwtSecretName = configuration["AzureKeyVault__JWTTokenSecretName"];
        _jwtSecretKey = configuration.GetSecret(jwtSecretName);
    }

    public async Task<string> GenerateToken(Users user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
