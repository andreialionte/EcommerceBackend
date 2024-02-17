namespace Ecommerce2.Services
{
    public interface IStorageService
    {
        string Upload(IFormFile file, string contentType);
        string GetImageUrl(string fileName);
    }
}
