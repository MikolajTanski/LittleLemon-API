// ImageController.cs

using LittleLemon_API.Services.ImageService;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageService _imageService;

    public ImageController(ILogger<ImageController> logger
        , IImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    [HttpGet("all-images")]
    public async Task<IActionResult> GetAllImagesAsync()
    {
        try
        {
            var images = await _imageService.GetAllImagesAsync();
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving image: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}

