using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
namespace MottuChallenge.API.Services.UseCases.DeliveryPeople
{
    public class CreateDeliveryPersonUseCase
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        public CreateDeliveryPersonUseCase(IDeliveryPersonRepository deliveryPersonRepository) => _deliveryPersonRepository = deliveryPersonRepository;

        public async Task<string> ExecuteAsync(CreateDeliveryPersonRequest request)
        {
            if (await _deliveryPersonRepository.GetByIdAsync(request.Identifier) != null)
                throw new Exception("Identificador já cadastrado.");
            if (await _deliveryPersonRepository.CnpjExistsAsync(request.Cnpj))
                throw new DuplicateCnpjException(request.Cnpj);
            if (await _deliveryPersonRepository.CnhNumberExistsAsync(request.CnhNumber))
                throw new DuplicateCnhNumberException(request.CnhNumber);
            if (!Enum.TryParse<CnhType>(request.CnhType, true, out var cnhType) || (cnhType != CnhType.A && cnhType != CnhType.B && cnhType != CnhType.AB))
                throw new InvalidCnhTypeException(request.CnhType);

            var deliveryPerson = new DeliveryPersonEntity
            {
                Identifier = request.Identifier,
                Name = request.Name,
                Cnpj = request.Cnpj,
                BirthDate = request.BirthDate.ToUniversalTime(),
                CnhNumber = request.CnhNumber,
                CnhType = cnhType
            };
            await _deliveryPersonRepository.AddAsync(deliveryPerson);
            return deliveryPerson.Identifier;
        }
    }
}