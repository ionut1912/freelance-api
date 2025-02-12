using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Frelance.Application.Repositories.External;
using Frelance.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Frelance.Infrastructure.Services.External;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        _blobServiceClient = new BlobServiceClient(configuration.GetSecret("storage-connection-string", "AzureKeyVault__StorageConnectionString"));
    }
    public async Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile blobFile)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var existingBlob = containerClient.GetBlobClient(blobItem.Name);
            return existingBlob.Uri.ToString();
        }

        var blobClient = containerClient.GetBlobClient(blobName);
        await using (var stream = blobFile.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.ToString();
    }

    public async Task DeleteBlobAsync(string containerName, string folderName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: folderName + "/"))
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}