namespace MottuChallenge.API.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, Guid deliveryPersonId);
    }
}