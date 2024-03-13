// ImageController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System;

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

    [HttpGet]
    public IActionResult GetImage()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageConnection");
            var containerName = "hero";
            var blobName = "restaurant-food.jpg";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var imageUrl = blobClient.Uri.ToString();

            return Ok(new { ImageUrl = imageUrl });

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving image: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

