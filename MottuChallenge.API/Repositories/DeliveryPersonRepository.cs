using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Infrastructure.Persistence;
namespace MottuChallenge.API.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly AppDbContext _context;
        public DeliveryPersonRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(DeliveryPersonEntity deliveryPerson)
        {
            await _context.DeliveryPeopleEntities.AddAsync(deliveryPerson);
            await _context.SaveChangesAsync();
        }

        public async Task<DeliveryPersonEntity?> GetByIdAsync(string identifier)
        {
            return await _context.DeliveryPeopleEntities.FindAsync(identifier);
        }

        public async Task<bool> CnpjExistsAsync(string cnpj)
        {
            return await _context.DeliveryPeopleEntities.AnyAsync(d => d.Cnpj == cnpj);
        }

        public async Task<bool> CnhNumberExistsAsync(string cnhNumber)
        {
            return await _context.DeliveryPeopleEntities.AnyAsync(d => d.CnhNumber == cnhNumber);
        }

        public async Task UpdateAsync(DeliveryPersonEntity deliveryPerson)
        {
            _context.DeliveryPeopleEntities.Update(deliveryPerson);
            await _context.SaveChangesAsync();
        }
    }
}