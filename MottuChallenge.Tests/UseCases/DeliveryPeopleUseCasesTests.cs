// =========================================================================
// ARQUIVO DE TESTE
// Local: MottuChallenge.Tests/UseCases/DeliveryPeople/DeliveryPersonUseCasesTests.cs
// =========================================================================
using Moq;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MottuChallenge.Tests.UseCases.DeliveryPeople
{
    public class DeliveryPersonUseCasesTests
    {
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepositoryMock;
        private readonly CreateDeliveryPersonUseCase _useCase;

        public DeliveryPersonUseCasesTests()
        {
            _deliveryPersonRepositoryMock = new Mock<IDeliveryPersonRepository>();
            _useCase = new CreateDeliveryPersonUseCase(_deliveryPersonRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_ShouldAddDeliveryPerson_WhenDataIsUnique()
        {
            // Arrange
            var request = new CreateDeliveryPersonRequestDTO
            {
                Name = "João Silva",
                Cnpj = "11222333000144",
                CnhNumber = "12345678901",
                BirthDate = new DateTime(1990, 1, 1),
                CnhType = "A"
            };

            _deliveryPersonRepositoryMock.Setup(r => r.CnpjExistsAsync(request.Cnpj)).ReturnsAsync(false);
            _deliveryPersonRepositoryMock.Setup(r => r.CnhNumberExistsAsync(request.CnhNumber)).ReturnsAsync(false);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _deliveryPersonRepositoryMock.Verify(r => r.AddAsync(It.IsAny<DeliveryPersonEntity>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenCnpjExists()
        {
            // Arrange
            var request = new CreateDeliveryPersonRequestDTO { Cnpj = "11222333000144" };
            _deliveryPersonRepositoryMock.Setup(r => r.CnpjExistsAsync(request.Cnpj)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenCnhExists()
        {
            // Arrange
            var request = new CreateDeliveryPersonRequestDTO { Cnpj = "11222333000144", CnhNumber = "12345678901" };
            _deliveryPersonRepositoryMock.Setup(r => r.CnpjExistsAsync(request.Cnpj)).ReturnsAsync(false);
            _deliveryPersonRepositoryMock.Setup(r => r.CnhNumberExistsAsync(request.CnhNumber)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenCnhTypeIsInvalid()
        {
            // Arrange
            var request = new CreateDeliveryPersonRequestDTO
            {
                Name = "João Silva",
                Cnpj = "11222333000144",
                CnhNumber = "12345678901",
                BirthDate = new DateTime(1990, 1, 1),
                CnhType = "C" // Tipo inválido
            };

            _deliveryPersonRepositoryMock.Setup(r => r.CnpjExistsAsync(request.Cnpj)).ReturnsAsync(false);
            _deliveryPersonRepositoryMock.Setup(r => r.CnhNumberExistsAsync(request.CnhNumber)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(request));
        }
    }
}
