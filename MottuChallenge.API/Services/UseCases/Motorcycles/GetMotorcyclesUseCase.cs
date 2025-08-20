using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class GetMotorcyclesUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public GetMotorcyclesUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task<IEnumerable<MotorcycleResponse>> ExecuteAsync()
        {
            var motorcycles = await _repo.GetAllAsync();
            return motorcycles.Select(m => new MotorcycleResponse { Identifier = m.Identifier, Year = m.Year, Model = m.Model, LicensePlate = m.LicensePlate });
        }
    }
}