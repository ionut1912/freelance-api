using System;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
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
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);

            if (!string.IsNullOrEmpty(secretName))
            {
                logger.LogInformation($"Fetching secret: {secretName} from {keyVaultUrl}");
                var secret = client.GetSecret(secretName);
                if (secret?.Value?.Value == null || string.IsNullOrEmpty(secret.Value.Value))
                {
                    logger.LogError($"Secret '{secretName}' is empty.");
                    throw new Exception($"Secret '{secretName}' retrieved from Azure Key Vault is empty.");
                }
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
            throw new Exception($"Failed to retrieve secret '{secretName}'", ex);
        }
    }
}
