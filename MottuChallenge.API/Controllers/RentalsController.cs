using Microsoft.AspNetCore.Mvc;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Services.UseCases.Rentals;

namespace MottuChallenge.API.Controllers
{
    [ApiController]
    [Route("locacao")]
    public class RentalsController : ControllerBase
    {
        private readonly CreateRentalUseCase _createRentalUseCase;
        private readonly UpdateRentalReturnDateUseCase _updateRentalReturnDateUseCase;
        private readonly GetRentalByIdUseCase _getRentalByIdUseCase;

        public RentalsController(CreateRentalUseCase create, UpdateRentalReturnDateUseCase update, GetRentalByIdUseCase getById)
        {
            _createRentalUseCase = create;
            _updateRentalReturnDateUseCase = update;
            _getRentalByIdUseCase = getById;
        }

        /// <summary>
        /// Cria uma nova locação de moto para um entregador.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalRequestDTO requestDto)
        {
            try
            {
                var rental = await _createRentalUseCase.ExecuteAsync(requestDto);
                return CreatedAtAction(nameof(Create), new { id = rental.Id }, rental);
            }
            catch (Exception ex) when (ex is DeliveryPersonNotFoundException or DeliveryPersonNotEligibleException or InvalidRentalPlanException)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex) when (ex is DeliveryPersonHasActiveRentalException or MotorcycleAlreadyRentedException)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        
        /// <summary>
        /// Consulta uma locação pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rental = await _getRentalByIdUseCase.ExecuteAsync(id);
            if (rental == null)
            {
                return NotFound(new { message = "Locação não encontrada." });
            }
            return Ok(rental);
        }
        
        /// <summary>
        /// Finaliza uma locação e calcula o custo final.
        /// </summary>
        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> UpdateReturnDate(Guid id, [FromBody] UpdateRentalReturnDateRequestDTO requestDto)
        {
            try
            {
                var result = await _updateRentalReturnDateUseCase.ExecuteAsync(id, requestDto);
                return Ok(result);
            }
            catch (RentalNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}