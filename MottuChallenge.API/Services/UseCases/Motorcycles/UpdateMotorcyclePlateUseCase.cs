using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class UpdateMotorcyclePlateUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public UpdateMotorcyclePlateUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task ExecuteAsync(Guid id, UpdateMotorcyclePlateRequestDTO requestDto)
        {
            var motorcycle = await _repo.GetByIdAsync(id) ?? throw new MotorcycleNotFoundException();
            var existing = await _repo.GetByLicensePlateAsync(requestDto.LicensePlate);
            if (existing != null && existing.Id != id)
                throw new DuplicateLicensePlateException(requestDto.LicensePlate);
            
            motorcycle.LicensePlate = requestDto.LicensePlate;
            await _repo.UpdateAsync(motorcycle);
        }
    }
}