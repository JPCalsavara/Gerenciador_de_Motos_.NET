using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class GetMotorcycleByIdUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public GetMotorcycleByIdUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task<MotorcycleResponse?> ExecuteAsync(string identifier)
        {
            var motorcycle = await _repo.GetByIdAsync(identifier);
            if (motorcycle == null) return null;
            return new MotorcycleResponse { Identifier = motorcycle.Identifier, Year = motorcycle.Year, Model = motorcycle.Model, LicensePlate = motorcycle.LicensePlate };
        }
    }
}