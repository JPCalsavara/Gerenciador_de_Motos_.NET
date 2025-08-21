using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class GetMotorcyclesUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public GetMotorcyclesUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task<IEnumerable<MotorcycleResponseDTO>> ExecuteAsync()
        {
            var motorcycles = await _repo.GetAllAsync();
            return motorcycles.Select(m => new MotorcycleResponseDTO { Id = m.Id, Year = m.Year, Model = m.Model, LicensePlate = m.LicensePlate });
        }
    }
}