using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Infrastructure.Persistence;
namespace MottuChallenge.API.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;
        public RentalRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(RentalEntity rental)
        {
            await _context.RentalEntities.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasActiveRentalAsync(Guid deliveryPersonId)
        {
            return await _context.RentalEntities
                .AnyAsync(r => r.DeliveryPersonId == deliveryPersonId && r.IsActive);
        }

        public async Task<RentalEntity?> GetActiveRentalByIdAsync(Guid id)
        {
            return await _context.RentalEntities
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }
        
        
        public async Task<RentalEntity?> GetByIdAsync(Guid id)
        {
            return await _context.RentalEntities.FindAsync(id);
        }
        
        public async Task UpdateAsync(RentalEntity rental)
        {
            _context.RentalEntities.Update(rental);
            await _context.SaveChangesAsync();
        }
    }
}