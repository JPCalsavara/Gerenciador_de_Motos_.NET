using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class GetMotorcycleByIdUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public GetMotorcycleByIdUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task<MotorcycleResponseDTO?> ExecuteAsync(Guid id)
        {
            var motorcycle = await _repo.GetByIdAsync(id);
            if (motorcycle == null) return null;
            return new MotorcycleResponseDTO { Id = motorcycle.Id, Year = motorcycle.Year, Model = motorcycle.Model, LicensePlate = motorcycle.LicensePlate };
        }
    }
}