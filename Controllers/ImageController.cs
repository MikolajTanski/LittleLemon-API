// ImageController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System;
using Azure.Storage.Blobs.Models;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IConfiguration configuration, ILogger<ImageController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("all-images")]
    public IActionResult GetAllImages()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageConnection");
            var containerName = "hero";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<object> images = new List<object>();

            foreach (BlobItem blobItem in containerClient.GetBlobs())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                images.Add(new {Name = blobItem.Name, Url = blobClient.Uri.ToString() });
            }

            return Ok(images);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving image: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

