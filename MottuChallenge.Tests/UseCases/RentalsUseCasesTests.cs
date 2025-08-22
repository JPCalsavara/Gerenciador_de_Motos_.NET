using Moq;
using MottuChallenge.API.DTOs;
using MottuChallenge.API.Entities;
using MottuChallenge.API.Exceptions;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services.UseCases.Rentals;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MottuChallenge.Tests.UseCases.Rentals
{
    public class RentalUseCasesTests
    {
        private readonly Mock<IRentalRepository> _rentalRepoMock;
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepoMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepoMock;

        public RentalUseCasesTests()
        {
            _rentalRepoMock = new Mock<IRentalRepository>();
            _deliveryPersonRepoMock = new Mock<IDeliveryPersonRepository>();
            _motorcycleRepoMock = new Mock<IMotorcycleRepository>();
        }

        [Fact]
        public async Task Create_ShouldCreateRental_WhenAllConditionsAreMet()
        {
            // Arrange
            var useCase = new CreateRentalUseCase(_rentalRepoMock.Object, _deliveryPersonRepoMock.Object, _motorcycleRepoMock.Object);
            var request = new CreateRentalRequestDTO { DeliveryPersonId = Guid.NewGuid(), PlanDays = 7 };
            var deliveryPerson = new DeliveryPersonEntity { Id = request.DeliveryPersonId, CnhType = CnhType.A };
            var motorcycle = new MotorcycleEntity { Id = Guid.NewGuid() };

            _deliveryPersonRepoMock.Setup(r => r.GetByIdAsync(request.DeliveryPersonId)).ReturnsAsync(deliveryPerson);
            _rentalRepoMock.Setup(r => r.HasActiveRentalAsync(request.DeliveryPersonId)).ReturnsAsync(false);
            _motorcycleRepoMock.Setup(r => r.GetAvailableAsync()).ReturnsAsync(motorcycle);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            _rentalRepoMock.Verify(r => r.AddAsync(It.IsAny<RentalEntity>()), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnRental_WhenExists()
        {
            // Arrange
            var useCase = new GetRentalByIdUseCase(_rentalRepoMock.Object);
            var rentalId = Guid.NewGuid();
            var rentalEntity = new RentalEntity { Id = rentalId };
            _rentalRepoMock.Setup(r => r.GetByIdAsync(rentalId)).ReturnsAsync(rentalEntity);

            // Act
            var result = await useCase.ExecuteAsync(rentalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rentalId, result.Id);
        }
    }

    public class UpdateRentalReturnDateUseCaseTests
    {
        private readonly Mock<IRentalRepository> _rentalRepoMock;
        private readonly UpdateRentalReturnDateUseCase _useCase;

        public UpdateRentalReturnDateUseCaseTests()
        {
            _rentalRepoMock = new Mock<IRentalRepository>();
            _useCase = new UpdateRentalReturnDateUseCase(_rentalRepoMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldCalculateWithPenalty_For7DayPlanEarlyReturn()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var startDate = new DateTime(2025, 8, 20);
            var rental = new RentalEntity { Id = rentalId, PlanDays = 7, StartDate = startDate, EndDate = startDate.AddDays(7) };
            var request = new UpdateRentalReturnDateRequestDTO { ReturnDate = startDate.AddDays(5) }; // 2 dias antes
            _rentalRepoMock.Setup(r => r.GetActiveRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _useCase.ExecuteAsync(rentalId, request);

            // Assert: (5 * 30) + (2 * 30 * 0.20) = 150 + 12 = 162
            Assert.Equal(162.00m, result.TotalCost);
        }

        [Fact]
        public async Task Execute_ShouldCalculateWithPenalty_For15DayPlanEarlyReturn()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var startDate = new DateTime(2025, 8, 20);
            var rental = new RentalEntity { Id = rentalId, PlanDays = 15, StartDate = startDate, EndDate = startDate.AddDays(15) };
            var request = new UpdateRentalReturnDateRequestDTO { ReturnDate = startDate.AddDays(10) }; // 5 dias antes
            _rentalRepoMock.Setup(r => r.GetActiveRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _useCase.ExecuteAsync(rentalId, request);

            // Assert: (10 * 28) + (5 * 28 * 0.40) = 280 + 56 = 336
            Assert.Equal(336.00m, result.TotalCost);
        }

        [Fact]
        public async Task Execute_ShouldCalculateWithoutPenalty_For30DayPlanEarlyReturn()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var startDate = new DateTime(2025, 8, 20);
            var rental = new RentalEntity { Id = rentalId, PlanDays = 30, StartDate = startDate, EndDate = startDate.AddDays(30) };
            var request = new UpdateRentalReturnDateRequestDTO { ReturnDate = startDate.AddDays(20) }; // 10 dias antes
            _rentalRepoMock.Setup(r => r.GetActiveRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _useCase.ExecuteAsync(rentalId, request);

            // Assert: (20 * 22) + 0 = 440
            Assert.Equal(440.00m, result.TotalCost);
        }

        [Fact]
        public async Task Execute_ShouldCalculateWithExtraFee_ForLateReturn()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var startDate = new DateTime(2025, 8, 20);
            var rental = new RentalEntity { Id = rentalId, PlanDays = 7, StartDate = startDate, EndDate = startDate.AddDays(7) };
            var request = new UpdateRentalReturnDateRequestDTO { ReturnDate = startDate.AddDays(9) }; // 2 dias depois
            _rentalRepoMock.Setup(r => r.GetActiveRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _useCase.ExecuteAsync(rentalId, request);

            // Assert: (7 * 30) + (2 * 50) = 210 + 100 = 310
            Assert.Equal(310.00m, result.TotalCost);
        }

        [Fact]
        public async Task Execute_ShouldCalculateCorrectly_ForOnTimeReturn()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var startDate = new DateTime(2025, 8, 20);
            var rental = new RentalEntity { Id = rentalId, PlanDays = 15, StartDate = startDate, EndDate = startDate.AddDays(15) };
            var request = new UpdateRentalReturnDateRequestDTO { ReturnDate = startDate.AddDays(15) }; // No dia exato
            _rentalRepoMock.Setup(r => r.GetActiveRentalByIdAsync(rentalId)).ReturnsAsync(rental);

            // Act
            var result = await _useCase.ExecuteAsync(rentalId, request);

            // Assert: 15 * 28 = 420
            Assert.Equal(420.00m, result.TotalCost);
        }
    }
}