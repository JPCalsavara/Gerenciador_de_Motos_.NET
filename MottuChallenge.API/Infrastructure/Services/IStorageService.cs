namespace MottuChallenge.API.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(byte[] fileBytes, string identifier, string contentType, string fileExtension);
    }
}