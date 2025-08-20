using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.DeliveryPeople
{
    public class UploadCnhUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IStorageService _storageService;

        public UploadCnhUseCase(IDeliveryPersonRepository deliveryPersonRepository, IStorageService storageService)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _storageService = storageService;
        }

        public async Task ExecuteAsync(string identifier, IFormFile file)
        {
            var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(identifier) ?? throw new DeliveryPersonNotFoundException();
            var allowedExtensions = new[] { ".png", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new InvalidFileTypeException("Apenas ficheiros .png e .bmp são permitidos.");

            var fileUrl = await _storageService.UploadFileAsync(file, identifier);
            deliveryPerson.CnhImageUrl = fileUrl;
            await _deliveryPersonRepository.UpdateAsync(deliveryPerson);
        }
    }
}