using MottuChallenge.API.Entities;
namespace MottuChallenge.API.Repositories
{
    public interface IMotorcycleRepository
    {
        Task AddAsync(MotorcycleEntity motorcycle);
        Task<MotorcycleEntity?> GetByIdAsync(string identifier);
        Task<MotorcycleEntity?> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<MotorcycleEntity>> GetAllAsync();
        Task UpdateAsync(MotorcycleEntity motorcycle);
        Task DeleteAsync(MotorcycleEntity motorcycle);
    }
}