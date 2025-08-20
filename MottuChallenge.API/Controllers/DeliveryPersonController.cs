using Microsoft.AspNetCore.Mvc;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;
namespace MottuChallenge.API.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class DeliveryPersonController : ControllerBase
    {
        private readonly CreateDeliveryPersonUseCase _create;
        private readonly UploadCnhUseCase _upload;

        public DeliveryPersonController(CreateDeliveryPersonUseCase create, UploadCnhUseCase upload)
        {
            _create = create;
            _upload = upload;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryPersonRequest request)
        {
            try
            {
                var id = await _create.ExecuteAsync(request);
                return CreatedAtAction(nameof(Create), new { id }, new { id });
            }
            catch (Exception ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpPost("{identifier}/cnh")]
        public async Task<IActionResult> UploadCnh(string identifier, IFormFile cnhImage)
        {
            try
            {
                await _upload.ExecuteAsync(identifier, cnhImage);
                return NoContent();
            }
            catch (DeliveryPersonNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidFileTypeException ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}