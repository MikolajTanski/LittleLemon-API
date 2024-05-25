namespace LittleLemon_API.Services.ImageService
{
    public interface IImageService
    {
        Task<List<object>> GetAllImagesAsync();
    }
}
