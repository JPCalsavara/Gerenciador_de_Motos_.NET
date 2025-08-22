using MottuChallenge.API.DTOs;
using MottuChallenge.API.Repositories;

namespace MottuChallenge.API.Services.UseCases.DeliveryPeople
{
    public class GetDeliveryPeopleUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public GetDeliveryPeopleUseCase(IDeliveryPersonRepository deliveryPersonRepository)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
        }

        public async Task<IEnumerable<DeliveryPersonResponseDTO>> ExecuteAsync()
        {
            var deliveryPeople = await _deliveryPersonRepository.GetAllAsync();

            // Mapeia a lista de entidades para uma lista de DTOs de resposta
            return deliveryPeople.Select(dp => new DeliveryPersonResponseDTO
            {
                Id = dp.Id,
                Name = dp.Name,
                Cnpj = dp.Cnpj,
                BirthDate = DateOnly.FromDateTime(dp.BirthDate),
                CnhNumber = dp.CnhNumber,
                CnhType = dp.CnhType.ToString(),
                CnhImageUrl = dp.CnhImageUrl
            });
        }
    }
}