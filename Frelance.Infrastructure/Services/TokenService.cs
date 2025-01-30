using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Frelance.Infrastructure.Entities;
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
        var keyVaultUrl = configuration["AzureKeyVault:VaultUrl"];
        var jwtSecretName = configuration["AzureKeyVault:JWTTokenSecretName"];

        if (string.IsNullOrEmpty(keyVaultUrl) || string.IsNullOrEmpty(jwtSecretName))
        {
            throw new Exception("Azure Key Vault configuration for JWT secret is missing.");
        }

        try
        {
            var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            var secret = client.GetSecret(jwtSecretName);
            _jwtSecretKey = secret.Value.Value;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve JWT secret key from Azure Key Vault", ex);
        }
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
