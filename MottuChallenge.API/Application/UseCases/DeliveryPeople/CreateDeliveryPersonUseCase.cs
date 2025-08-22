using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;

namespace MottuChallenge.API.Application.UseCases.DeliveryPeople
{
    public class CreateDeliveryPersonUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        public CreateDeliveryPersonUseCase(IDeliveryPersonRepository deliveryPersonRepository) => _deliveryPersonRepository = deliveryPersonRepository;

        public async Task<Guid> ExecuteAsync(CreateDeliveryPersonRequestDTO requestDto)
        {
            if (await _deliveryPersonRepository.CnpjExistsAsync(requestDto.Cnpj))
                throw new DuplicateCnpjException(requestDto.Cnpj);
            if (await _deliveryPersonRepository.CnhNumberExistsAsync(requestDto.CnhNumber))
                throw new DuplicateCnhNumberException(requestDto.CnhNumber);
            if (!Enum.TryParse<CnhType>(requestDto.CnhType, true, out var cnhType) || (cnhType != CnhType.A && cnhType != CnhType.B && cnhType != CnhType.AB))
                throw new InvalidCnhTypeException(requestDto.CnhType);

            var deliveryPerson = new DeliveryPersonEntity
            {
                Name = requestDto.Name,
                Cnpj = requestDto.Cnpj,
                BirthDate = requestDto.BirthDate.ToUniversalTime(),
                CnhNumber = requestDto.CnhNumber,
                CnhType = cnhType
            };
            await _deliveryPersonRepository.AddAsync(deliveryPerson);

            return deliveryPerson.Id;
        }
    }
}