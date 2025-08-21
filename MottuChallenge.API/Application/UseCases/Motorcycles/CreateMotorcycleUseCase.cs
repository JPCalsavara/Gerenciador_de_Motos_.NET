using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services; // Importar o serviço de mensageria

namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class CreateMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _repo;
        private readonly IMessagingService _messagingService;

        public CreateMotorcycleUseCase(IMotorcycleRepository repo, IMessagingService messagingService)
        {
            _repo = repo;
            _messagingService = messagingService;
        }

        public async Task<Guid> ExecuteAsync(CreateMotorcycleRequestDTO request)
        {
            if (await _repo.GetByLicensePlateAsync(request.LicensePlate) != null)
                throw new Exception("Placa já cadastrada.");

            var motorcycle = new MotorcycleEntity
            {
                Year = request.Year,
                Model = request.Model,
                LicensePlate = request.LicensePlate
            };
            
            await _repo.AddAsync(motorcycle);

            // --- PUBLICAR MENSAGEM ---
            _messagingService.Publish("motorcycle-created", motorcycle);

            return motorcycle.Id;
        }
    }
}