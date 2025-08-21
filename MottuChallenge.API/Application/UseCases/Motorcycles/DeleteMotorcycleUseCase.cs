using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Motorcycles
{
    public class DeleteMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _repo;
        public DeleteMotorcycleUseCase(IMotorcycleRepository repo) => _repo = repo;

        public async Task ExecuteAsync(Guid id)
        {
            var motorcycle = await _repo.GetByIdAsync(id) ?? throw new MotorcycleNotFoundException();
            await _repo.DeleteAsync(motorcycle);
        }
    }
}