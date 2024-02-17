using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Ecommerce2.Services
{
    public class StorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public StorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
        }

        public string Upload(IFormFile formFile, string contentType)
        {
            var containerName = _configuration.GetSection("Storage:ContainerName").Value;
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var getBlobClient = containerClient.GetBlobClient(formFile.FileName);
            var blobUploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            };

            using var stream = formFile.OpenReadStream();
            getBlobClient.Upload(stream, blobUploadOptions);

            return formFile.FileName;
        }
        public string GetImageUrl(string fileName)
        {
            return $"https://projectswithphotos.blob.core.windows.net/ecommerce/{fileName}";
        }
    }
}