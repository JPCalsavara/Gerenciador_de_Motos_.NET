using Microsoft.AspNetCore.Mvc;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;

namespace MottuChallenge.API.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class DeliveryPeopleController : ControllerBase
    {
        private readonly CreateDeliveryPersonUseCase _createDeliveryPersonUseCase;
        // private readonly UploadCnhUseCase _uploadCnhUseCase; // Dependência removida por agora

        public DeliveryPeopleController(CreateDeliveryPersonUseCase createDeliveryPersonUseCase/*, UploadCnhUseCase uploadCnhUseCase*/)
        {
            _createDeliveryPersonUseCase = createDeliveryPersonUseCase;
            // _uploadCnhUseCase = uploadCnhUseCase; // Injeção removida por agora
        }

        /// <summary>
        /// Cadastrar novo entregador
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryPersonRequestDTO request)
        {
            try
            {
                var driverId = await _createDeliveryPersonUseCase.ExecuteAsync(request);
                return CreatedAtAction(nameof(Create), new { id = driverId }, new { id = driverId });
            }
            catch (Exception ex) when (ex is DuplicateCnpjException || ex is DuplicateCnhNumberException)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (InvalidCnhTypeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /*
        /// <summary>
        /// Enviar foto da CNH (FUNCIONALIDADE DESATIVADA TEMPORARIAMENTE)
        /// </summary>
        [HttpPost("{id}/cnh")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadCnh(Guid id, IFormFile cnhImage)
        {
            try
            {
                await _uploadCnhUseCase.ExecuteAsync(id, cnhImage);
                return NoContent();
            }
            catch (DeliveryPersonNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidFileTypeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        */
    }
}