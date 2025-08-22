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

        public async Task<string> UploadFileAsync(byte[] fileBytes, string identifier, string contentType, string fileExtension)
        {
            var bucketName = _configuration["AWS:BucketName"];
            var region = _configuration["AWS:Region"];
            var key = $"cnh-images/{identifier}_{Guid.NewGuid()}{fileExtension}";

            using var memoryStream = new MemoryStream(fileBytes);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = memoryStream,
                ContentType = contentType
                // A linha CannedACL foi removida para ser compatível com as novas políticas de bucket
            };

            await _s3Client.PutObjectAsync(request);

            return $"https://{bucketName}.s3.{region}.amazonaws.com/{key}";
        }
    }
}