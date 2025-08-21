using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Rentals
{
    public class CreateRentalUseCase
    {
        private readonly IRentalRepository _rentalRepo;
        private readonly IDeliveryPersonRepository _deliveryPersonRepo;
        private readonly IMotorcycleRepository _motorcycleRepo;

        public CreateRentalUseCase(IRentalRepository rentalRepo, IDeliveryPersonRepository deliveryPersonRepo, IMotorcycleRepository motorcycleRepo)
        {
            _rentalRepo = rentalRepo;
            _deliveryPersonRepo = deliveryPersonRepo;
            _motorcycleRepo = motorcycleRepo;
        }

        public async Task<RentalEntity> ExecuteAsync(CreateRentalRequestDTO requestDto)
        {
            var deliveryPerson = await _deliveryPersonRepo.GetByIdAsync(requestDto.DeliveryPersonId) ?? throw new DeliveryPersonNotFoundException();
            if (deliveryPerson.CnhType != CnhType.A && deliveryPerson.CnhType != CnhType.AB)
                throw new DeliveryPersonNotEligibleException();

            if (await _rentalRepo.HasActiveRentalAsync(requestDto.DeliveryPersonId))
                throw new DeliveryPersonHasActiveRentalException();

            if (await _motorcycleRepo.IsRentedAsync(requestDto.MotorcycleId))
                throw new MotorcycleAlreadyRentedException();

            var dailyRate = GetDailyRate(requestDto.PlanDays);

            var rental = new RentalEntity
            {
                DeliveryPersonId = deliveryPerson.Id,
                MotorcycleId = requestDto.MotorcycleId,
                PlanDays = requestDto.PlanDays,
                StartDate = requestDto.StartDate.ToUniversalTime(),
                EndDate = requestDto.EndDate.ToUniversalTime(),
                PredictedEndDate = requestDto.PredictedEndDate.ToUniversalTime(),
                TotalCost = requestDto.PlanDays * dailyRate
            };

            await _rentalRepo.AddAsync(rental);
            return rental;
        }

        private decimal GetDailyRate(int planDays) => planDays switch
        {
            7 => 30.00m, 15 => 28.00m, 30 => 22.00m, 45 => 20.00m, 50 => 18.00m,
            _ => throw new InvalidRentalPlanException()
        };
    }
}