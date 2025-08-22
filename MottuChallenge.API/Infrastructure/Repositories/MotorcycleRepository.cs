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

        public async Task<MotorcycleEntity?> GetByIdAsync(Guid identifier)
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
        
        public async Task<bool> IsRentedAsync(Guid identifier)
        {
            return await _context.RentalEntities.AnyAsync(r => r.MotorcycleId == identifier && r.IsActive);
        }

        public async Task<bool> HasRentalsAsync(Guid identifier)
        {
            return await _context.RentalEntities.AnyAsync(r => r.MotorcycleId == identifier);
        }
        
        public async Task<MotorcycleEntity?> GetAvailableAsync()
        {
            // Encontra a primeira moto que não tenha um ID correspondente
            // na tabela de locações ativas.
            return await _context.MotorcycleEntities
                .FirstOrDefaultAsync(m => !_context.RentalEntities.Any(r => r.MotorcycleId == m.Id && r.IsActive));
        }
    }
}