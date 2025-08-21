using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.Rentals
{
    public class UpdateRentalReturnDateUseCase
    {
        private readonly IRentalRepository _rentalRepo;
        public UpdateRentalReturnDateUseCase(IRentalRepository rentalRepo) => _rentalRepo = rentalRepo;

        public async Task<CalculateReturnCostResponseDTO> ExecuteAsync(Guid id, UpdateRentalReturnDateRequestDTO requestDto)
        {
            var rental = await _rentalRepo.GetActiveRentalByIdAsync(id) ?? throw new RentalNotFoundException();
            var returnDate = requestDto.ReturnDate.ToUniversalTime();
            var dailyRate = GetDailyRate(rental.PlanDays);
            decimal finalCost = 0;
            string details = "";

            var daysUsed = (returnDate.Date - rental.StartDate.Date).Days;
            
            // Cenário 1: Devolução antecipada
            if (returnDate.Date < rental.EndDate.Date)
            {
                var unusedDays = (rental.EndDate.Date - returnDate.Date).Days;
                decimal penalty = 0;

                // --- LÓGICA DE MULTA ATUALIZADA ---
                switch (rental.PlanDays)
                {
                    case 7:
                        penalty = (unusedDays * dailyRate) * 0.20m;
                        break;
                    case 15:
                        penalty = (unusedDays * dailyRate) * 0.40m;
                        break;
                    default: // Para planos de 30, 45, 50 dias sem multa
                        break;
                }
                
                finalCost = (daysUsed * dailyRate) + penalty;
                details = $"Devolução antecipada. Custo: {daysUsed} dias * R${dailyRate:F2} + Multa de R${penalty:F2}.";
            }
            // Cenário 2: Devolução no prazo ou atrasada
            else
            {
                var extraDays = (returnDate.Date - rental.EndDate.Date).Days;
                var extraFee = extraDays > 0 ? extraDays * 50.00m : 0;
                finalCost = (rental.PlanDays * dailyRate) + extraFee;
                details = $"Custo do plano: {rental.PlanDays} dias * R${dailyRate:F2}.";
                if (extraDays > 0)
                    details += $" Custo adicional por atraso: {extraDays} dias * R$50.00 = R${extraFee:F2}.";
            }

            rental.IsActive = false;
            rental.TotalCost = finalCost;
            await _rentalRepo.UpdateAsync(rental);

            return new CalculateReturnCostResponseDTO { TotalCost = finalCost, Details = details };
        }
        
        private decimal GetDailyRate(int planDays) => planDays switch 
        { 
            7 => 30.00m, 
            15 => 28.00m, 
            30 => 22.00m, 
            45 => 20.00m, 
            50 => 18.00m, 
            _ => throw new InvalidRentalPlanException() 
        };
    }
}