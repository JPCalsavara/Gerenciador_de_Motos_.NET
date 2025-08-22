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

        public async Task ExecuteAsync(Guid id, string cnhImageBase64)
        {
            var deliveryPerson = await _deliveryPersonRepository.GetByIdAsync(id) ?? throw new DeliveryPersonNotFoundException();

            // Descodifica a string Base64 para bytes
            var fileBytes = Convert.FromBase64String(cnhImageBase64);

            // Valida o tipo de ficheiro (assumindo PNG por defeito, conforme o requisito)
            // Uma implementação mais robusta poderia validar os "magic bytes" do ficheiro.
            var fileUrl = await _storageService.UploadFileAsync(fileBytes, id.ToString(), "image/png", ".png");
            
            deliveryPerson.CnhImageUrl = fileUrl;

            await _deliveryPersonRepository.UpdateAsync(deliveryPerson);
        }
    }
}