using Amazon.S3;
using Amazon.S3.Model;

namespace MottuChallenge.API.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string deliveryPersonId)
        {
            var bucketName = _configuration["AWS:BucketName"];
            var fileExtension = Path.GetExtension(file.FileName);
            var key = $"cnh-images/{deliveryPersonId}_{Guid.NewGuid()}{fileExtension}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = memoryStream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead // Torna o arquivo publicamente acessível
            };

            await _s3Client.PutObjectAsync(request);

            // Retorna a URL pública do arquivo no S3
            return $"https://{bucketName}.s3.amazonaws.com/{key}";
        }
    }
}