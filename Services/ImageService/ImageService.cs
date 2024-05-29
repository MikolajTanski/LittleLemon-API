using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace LittleLemon_API.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<object>> GetAllImagesAsync()
        {
            var connectionStringBlob = _configuration["LittleLemonImages"];
            var containerName = "hero";

            var blobServiceClient = new BlobServiceClient(connectionStringBlob);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<object> images = new List<object>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                images.Add(new { Name = blobItem.Name, Url = blobClient.Uri.ToString() });
            }

            return images;
        }
    }
}
