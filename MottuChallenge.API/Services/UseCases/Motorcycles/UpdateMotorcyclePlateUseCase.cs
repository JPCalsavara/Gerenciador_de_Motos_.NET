using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class UpdateMotorcyclePlateUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public UpdateMotorcyclePlateUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task ExecuteAsync(string identifier, UpdateMotorcyclePlateRequest request)
        {
            var motorcycle = await _repo.GetByIdAsync(identifier) ?? throw new MotorcycleNotFoundException();
            var existing = await _repo.GetByLicensePlateAsync(request.LicensePlate);
            if (existing != null && existing.Identifier != identifier)
                throw new DuplicateLicensePlateException(request.LicensePlate);
            
            motorcycle.LicensePlate = request.LicensePlate;
            await _repo.UpdateAsync(motorcycle);
        }
    }
}