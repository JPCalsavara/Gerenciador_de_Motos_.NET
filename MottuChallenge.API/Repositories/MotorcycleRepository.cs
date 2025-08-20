using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Infrastructure.Persistence;
namespace MottuChallenge.API.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly AppDbContext _context;
        public MotorcycleRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(MotorcycleEntity motorcycle)
        {
            await _context.MotorcycleEntities.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task<MotorcycleEntity?> GetByIdAsync(string identifier)
        {
            return await _context.MotorcycleEntities.FindAsync(identifier);
        }

        public async Task<MotorcycleEntity?> GetByLicensePlateAsync(string licensePlate)
        {
            return await _context.MotorcycleEntities.FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
        }

        public async Task<IEnumerable<MotorcycleEntity>> GetAllAsync()
        {
            return await _context.MotorcycleEntities.ToListAsync();
        }

        public async Task UpdateAsync(MotorcycleEntity motorcycle)
        {
            _context.MotorcycleEntities.Update(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MotorcycleEntity motorcycle)
        {
            _context.MotorcycleEntities.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }
    }
}