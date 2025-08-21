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

        public async Task<Guid> ExecuteAsync(CreateMotorcycleRequestDTO requestDto)
        {
            if (await _motorcycleRepository.GetByLicensePlateAsync(requestDto.LicensePlate) != null)
                throw new DuplicateLicensePlateException(requestDto.LicensePlate);

            var motorcycle = new MotorcycleEntity
            {
                Year = requestDto.Year,
                Model = requestDto.Model,
                LicensePlate = requestDto.LicensePlate
            };
            await _motorcycleRepository.AddAsync(motorcycle);
            // Adicioanar mensageria
            
            return motorcycle.Id;
        }
    }
}