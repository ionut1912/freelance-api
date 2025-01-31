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
        var keyVaultUrl = configuration["AzureKeyVault__VaultUrl"];
        if (string.IsNullOrEmpty(keyVaultUrl))
        {
            throw new Exception("Azure Key Vault URL is missing from configuration.");
        }

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("AzureSecretExtension");

        try
        {
            var credential = new ManagedIdentityCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);

            if (!string.IsNullOrEmpty(secretName))
            {
                var secret = client.GetSecret(secretName);
                logger.LogInformation($"Successfully retrieved secret: {secretName}");
                return secret.Value.Value;
            }
            else
            {
                throw new Exception($"Secret name '{secretName}' is missing.");
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
    }
}
