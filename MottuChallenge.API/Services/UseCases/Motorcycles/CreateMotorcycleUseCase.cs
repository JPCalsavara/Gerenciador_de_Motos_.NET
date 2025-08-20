using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class CreateMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        public CreateMotorcycleUseCase(IMotorcycleRepository repo) => _motorcycleRepository = repo;

        public async Task<string> ExecuteAsync(CreateMotorcycleRequest request)
        {
            if (await _motorcycleRepository.GetByIdAsync(request.Identifier) != null)
                throw new Exception("Identificador já cadastrado.");
            if (await _motorcycleRepository.GetByLicensePlateAsync(request.LicensePlate) != null)
                throw new DuplicateLicensePlateException(request.LicensePlate);

            var motorcycle = new MotorcycleEntity
            {
                Identifier = request.Identifier,
                Year = request.Year,
                Model = request.Model,
                LicensePlate = request.LicensePlate
            };
            await _motorcycleRepository.AddAsync(motorcycle);
            // Adicioanar mensageria
            return motorcycle.Identifier;
        }
    }
}