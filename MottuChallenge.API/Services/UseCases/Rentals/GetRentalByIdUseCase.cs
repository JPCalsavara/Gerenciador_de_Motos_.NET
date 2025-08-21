using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Rentals
{
    public class GetRentalByIdUseCase
    {
        private readonly IRentalRepository _rentalRepo;
        public GetRentalByIdUseCase(IRentalRepository rentalRepo) => _rentalRepo = rentalRepo;

        public async Task<RentalResponseDTO?> ExecuteAsync(Guid id)
        {
            var rental = await _rentalRepo.GetByIdAsync(id);
            if (rental == null) return null;

            return new RentalResponseDTO
            {
                Id = rental.Id,
                DeliveryPersonIdentifier = rental.DeliveryPersonId,
                MotorcycleIdentifier = rental.MotorcycleId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                PredictedEndDate = rental.PredictedEndDate,
                PlanDays = rental.PlanDays,
                TotalCost = rental.TotalCost,
                IsActive = rental.IsActive
            };
        }
    }
}