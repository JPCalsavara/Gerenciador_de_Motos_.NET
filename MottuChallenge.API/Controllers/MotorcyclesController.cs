using Microsoft.AspNetCore.Mvc;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Services.UseCases.Motorcycles;
namespace MottuChallenge.API.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly CreateMotorcycleUseCase _create;
        private readonly GetMotorcyclesUseCase _getAll;
        private readonly GetMotorcycleByIdUseCase _getById;
        private readonly UpdateMotorcyclePlateUseCase _update;
        private readonly DeleteMotorcycleUseCase _delete;

        public MotorcyclesController(CreateMotorcycleUseCase create, GetMotorcyclesUseCase getAll, GetMotorcycleByIdUseCase getById, UpdateMotorcyclePlateUseCase update, DeleteMotorcycleUseCase delete)
        {
            _create = create;
            _getAll = getAll;
            _getById = getById;
            _update = update;
            _delete = delete;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request)
        {
            try
            {
                var id = await _create.ExecuteAsync(request);
                return CreatedAtAction(nameof(GetById), new { identifier = id }, new { id });
            }
            catch (Exception ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var motorcycles = await _getAll.ExecuteAsync();
            return Ok(motorcycles);
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetById(string identifier)
        {
            var motorcycle = await _getById.ExecuteAsync(identifier);
            if (motorcycle == null) return NotFound();
            return Ok(motorcycle);
        }

        [HttpPut("{identifier}/placa")]
        public async Task<IActionResult> UpdatePlate(string identifier, [FromBody] UpdateMotorcyclePlateRequest request)
        {
            try
            {
                await _update.ExecuteAsync(identifier, request);
                return NoContent();
            }
            catch (MotorcycleNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (Exception ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpDelete("{identifier}")]
        public async Task<IActionResult> Delete(string identifier)
        {
            try
            {
                await _delete.ExecuteAsync(identifier);
                return NoContent();
            }
            catch (MotorcycleNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }
    }
}