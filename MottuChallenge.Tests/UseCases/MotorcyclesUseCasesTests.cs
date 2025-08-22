using Moq;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services;
using MottuChallenge.API.Services.UseCases.Motorcycles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MottuChallenge.Tests.UseCases.Motorcycles
{
    public class MotorcycleUseCasesTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IMessagingService> _messagingServiceMock;

        public MotorcycleUseCasesTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _messagingServiceMock = new Mock<IMessagingService>();
        }

        [Fact]
        public async Task Create_ShouldAddMotorcycle_WhenPlateIsUnique()
        {
            // Arrange
            var request = new CreateMotorcycleRequestDTO { LicensePlate = "NEW-1234", Model = "Test Model", Year = 2024 };
            _motorcycleRepositoryMock.Setup(r => r.GetByLicensePlateAsync(request.LicensePlate)).ReturnsAsync((MotorcycleEntity?)null);
            var useCase = new CreateMotorcycleUseCase(_motorcycleRepositoryMock.Object, _messagingServiceMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _motorcycleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<MotorcycleEntity>()), Times.Once);
            _messagingServiceMock.Verify(m => m.Publish(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity> { new MotorcycleEntity(), new MotorcycleEntity() };
            _motorcycleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(motorcycles);
            var useCase = new GetMotorcyclesUseCase(_motorcycleRepositoryMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnMotorcycle_WhenExists()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var motorcycle = new MotorcycleEntity { Id = motorcycleId };
            _motorcycleRepositoryMock.Setup(r => r.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);
            var useCase = new GetMotorcycleByIdUseCase(_motorcycleRepositoryMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(motorcycleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motorcycleId, result.Id);
        }
    }
}