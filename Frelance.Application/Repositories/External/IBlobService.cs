using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Repositories.External;

public interface IBlobService
{
    Task<string> UploadBlobAsync(string containerName, string blobName, IFormFile blobFile);
    Task DeleteBlobAsync(string containerName,string folderName);
}