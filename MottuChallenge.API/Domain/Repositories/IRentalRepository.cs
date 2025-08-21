using MottuChallenge.API.Entities;
namespace MottuChallenge.API.Repositories
{
    public interface IRentalRepository
    {
        Task AddAsync(RentalEntity rental);
        Task<bool> HasActiveRentalAsync(Guid deliveryPersonIdentifier);
        Task<RentalEntity?> GetActiveRentalByIdAsync(Guid id);
        Task<RentalEntity?> GetByIdAsync(Guid id); // Novo método
        Task UpdateAsync(RentalEntity rental);
    }
}