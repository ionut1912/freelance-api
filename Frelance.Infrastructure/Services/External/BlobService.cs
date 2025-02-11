using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Frelance.Application.Repositories.External;
using Frelance.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Frelance.Infrastructure.Services.External;

public class BlobService : IBlobService
{
    private readonly IConfiguration _configuration;

    public BlobService(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _configuration = configuration;
    }
    public async Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile blobFile)
    {
        BlobServiceClient blobServiceClient =
            new BlobServiceClient(_configuration.GetSecret("storage-connection-string", "AzureKeyVault__StorageConnectionString"));
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            BlobClient existingBlob = containerClient.GetBlobClient(blobItem.Name);
            return existingBlob.Uri.ToString();
        }

        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        await using (var stream = blobFile.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.ToString();
    }
}