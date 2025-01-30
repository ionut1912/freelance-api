using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Frelance.Infrastructure.Extensions;

public static class AzureSecretExtension
{
    public static string GetSecret(this IConfiguration configuration,string secretName)
    {
        var secretValue=string.Empty;
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeAzureCliCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeManagedIdentityCredential = false,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeAzureDeveloperCliCredential = true
        });
        var keyVaultUrl = configuration["AzureKeyVault:Url"];
        if (!string.IsNullOrEmpty(keyVaultUrl))
        {
            try
            {
                var client = new SecretClient(new Uri(keyVaultUrl), credential);
                if (!string.IsNullOrEmpty(secretName))
                {
                    secretValue= client.GetSecret(secretName).Value.Value;
                }
                else
                {
                    throw new Exception($"{secretName} is missing from Azure Key Vault configuration.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve secrets from Azure Key Vault", ex);
            }
        }
        else
        {
            throw new Exception("Azure Key Vault configuration is missing.");
        }
        return secretValue;

    }
    
}