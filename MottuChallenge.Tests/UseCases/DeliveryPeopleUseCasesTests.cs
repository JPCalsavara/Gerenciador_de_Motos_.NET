using Moq;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MottuChallenge.API.Application.UseCases.DeliveryPeople;
using Xunit;

namespace MottuChallenge.Tests.UseCases.DeliveryPeople
{
    public class DeliveryPersonUseCasesTests
    {
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepositoryMock;
        private readonly Mock<IStorageService> _storageServiceMock;

        public DeliveryPersonUseCasesTests()
        {
            _deliveryPersonRepositoryMock = new Mock<IDeliveryPersonRepository>();
            _storageServiceMock = new Mock<IStorageService>();
        }

        [Fact]
        public async Task Create_ShouldAddDeliveryPerson_WhenDataIsUnique()
        {
            // Arrange
            var useCase = new CreateDeliveryPersonUseCase(_deliveryPersonRepositoryMock.Object);
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
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _deliveryPersonRepositoryMock.Verify(r => r.AddAsync(It.IsAny<DeliveryPersonEntity>()), Times.Once);
        }

        
        [Fact]
        public async Task UploadCnh_ShouldUpdateImageUrl_WhenFileIsValid()
        {
            // Arrange
            var useCase = new UploadCnhUseCase(_deliveryPersonRepositoryMock.Object, _storageServiceMock.Object);
            var personId = Guid.NewGuid();
            var deliveryPerson = new DeliveryPersonEntity { Id = personId };
            var base64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=";
            var fileUrl = "http://example.com/image.png";

            _deliveryPersonRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync(deliveryPerson);
            // --- CORREÇÃO AQUI ---
            // A simulação agora corresponde à nova assinatura do método UploadFileAsync
            _storageServiceMock.Setup(s => s.UploadFileAsync(
                    It.IsAny<byte[]>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>()))
                .ReturnsAsync(fileUrl);

            // Act
            await useCase.ExecuteAsync(personId, base64Image);

            // Assert
            _deliveryPersonRepositoryMock.Verify(r => r.UpdateAsync(It.Is<DeliveryPersonEntity>(p => p.CnhImageUrl == fileUrl)), Times.Once);
        }
    }
}