using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Frelance.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Frelance.Infrastructure.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Users> _userManager;

    public TokenService(IConfiguration configuration, UserManager<Users> userManager)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _configuration = configuration;
        ArgumentNullException.ThrowIfNull(userManager,nameof(userManager));
        _userManager = userManager;
    }

    public async Task<string> GenerateToken(Users user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach(var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:TokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenOptions=new JwtSecurityToken(null,null, claims,expires: DateTime.Now.AddDays(7),signingCredentials:creds);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);

    }
}