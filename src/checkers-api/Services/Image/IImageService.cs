namespace checkers_api.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile image);
}