using MottuChallenge.API.Entities;
namespace MottuChallenge.API.Repositories
{
    public interface IDeliveryPersonRepository
    {
        Task AddAsync(DeliveryPersonEntity deliveryPerson);
        Task<DeliveryPersonEntity?> GetByIdAsync(Guid identifier);
        Task<bool> CnpjExistsAsync(string cnpj);
        Task<bool> CnhNumberExistsAsync(string cnhNumber);
        Task UpdateAsync(DeliveryPersonEntity deliveryPerson);
    }
}