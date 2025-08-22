using Microsoft.AspNetCore.Mvc;
using MottuChallenge.API.Application.UseCases.DeliveryPeople;
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
        private readonly UploadCnhUseCase _uploadCnhUseCase; // Dependência removida por agora
        private readonly GetDeliveryPeopleUseCase _getDeliveryPeopleUseCase; // Nova dependência


        public DeliveryPeopleController(CreateDeliveryPersonUseCase createDeliveryPersonUseCase, UploadCnhUseCase uploadCnhUseCase, GetDeliveryPeopleUseCase getDeliveryPeopleUseCase)
        {
            _createDeliveryPersonUseCase = createDeliveryPersonUseCase;
            _uploadCnhUseCase = uploadCnhUseCase; // Injeção removida por agora
            _getDeliveryPeopleUseCase = getDeliveryPeopleUseCase;
            
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
        
        /// <summary>
        /// Consultar todos os entregadores
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryPersonResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var deliveryPeople = await _getDeliveryPeopleUseCase.ExecuteAsync();
            return Ok(deliveryPeople);
        }
        
        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        /// <remarks>
        /// Envie a imagem da CNH como uma string Base64 no corpo da requisição.
        /// </remarks>
        [HttpPost("{id:guid}/cnh")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadCnh(Guid id, [FromBody] UploadCnhRequestDTO request)
        {
            try
            {
                await _uploadCnhUseCase.ExecuteAsync(id, request.CnhImageBase64);
                return NoContent();
            }
            catch (DeliveryPersonNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (FormatException ex) // Captura erros de Base64 inválida
            {
                return BadRequest(new { message = "A string Base64 fornecida é inválida.", details = ex.Message });
            }
        }
        
    }
}