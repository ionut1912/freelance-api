using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Frelance.Infrastructure.Extensions;

public static class AzureSecretExtension
{
    public static string GetSecret(this IConfiguration configuration, string secretName)
    {
        var secretValue = string.Empty;
        var keyVaultUrl = configuration["AzureKeyVault__VaultUrl"];
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("AzureSecretExtension");

        if (string.IsNullOrEmpty(keyVaultUrl))
        {
            throw new Exception("Azure Key Vault URL is missing from configuration.");
        }

        try
        {
            var credential = new ManagedIdentityCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
            logger.LogInformation($"Key vault url is {keyVaultUrl}");
            if (!string.IsNullOrEmpty(secretName))
            {
                secretValue = client.GetSecret(secretName).Value.Value;
                logger.LogInformation($"Successfully retrieved secret: {secretName}");
            }
            else
            {
                throw new Exception($"{secretName} is missing from Azure Key Vault configuration.");
            }
        }
        catch (AuthenticationFailedException authEx)
        {
            logger.LogError($"Authentication to Azure Key Vault failed: {authEx.Message}");
            throw new Exception("Failed to authenticate to Azure Key Vault.", authEx);
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed to retrieve secret '{secretName}': {ex.Message}");
            throw new Exception("Failed to retrieve secrets from Azure Key Vault", ex);
        }

        return secretValue;
    }
}

public class TokenCredential : Azure.Core.TokenCredential
{
    private readonly string _accessToken;

    public TokenCredential(string accessToken)
    {
        _accessToken = accessToken;
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new AccessToken(_accessToken, DateTimeOffset.UtcNow.AddHours(1));
    }

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new ValueTask<AccessToken>(new AccessToken(_accessToken, DateTimeOffset.UtcNow.AddHours(1)));
    }
}
