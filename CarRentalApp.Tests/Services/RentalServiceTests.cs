using CarRentalApp.Application.DTOs;
using CarRentalApp.Application.Services;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using FluentAssertions;
using Moq;


namespace CarRentalApp.Tests.Services
{
    public class RentalServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RentalService _rentalService;

        public RentalServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _rentalService = new RentalService(_unitOfWorkMock.Object);
        }

        #region CREATE 

        [Fact]
        public async Task CreateRentalAsync_HappyPath_ReturnsRentalId()
        {
            var createDto = new CreateRentalDto
            {
                CarId = 1,
                ClientId = 1,
                RentalDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(2),
                IsPaid = true
            };

            var validCar = new Car { Id = 1, IsAvailable = true, PricePerDay = 100 };
            
            _unitOfWorkMock.Setup(u => u.Cars.GetByIdAsync(createDto.CarId))
                           .ReturnsAsync(validCar);

            _unitOfWorkMock.Setup(u => u.Rentals.GetRentalsByCarIdAsync(createDto.CarId))
                           .ReturnsAsync(new List<Rental>()); 

            _unitOfWorkMock.Setup(u => u.Rentals.AddAsync(It.IsAny<Rental>()))
                           .Callback<Rental>(r => r.Id = 99)
                           .Returns(Task.CompletedTask);

            var result = await _rentalService.CreateRentalAsync(createDto);

            result.Should().Be(99);
            _unitOfWorkMock.Verify(u => u.Rentals.AddAsync(It.IsAny<Rental>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateRentalAsync_CarIsUnavailable_ThrowsInvalidOperationException()
        {
            var createDto = new CreateRentalDto { CarId = 10 };
            var brokenCar = new Car { Id = 10, IsAvailable = false, PricePerDay = 50 };

            _unitOfWorkMock.Setup(u => u.Cars.GetByIdAsync(createDto.CarId))
                           .ReturnsAsync(brokenCar);

            Func<Task> act = async () => await _rentalService.CreateRentalAsync(createDto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("This car is unavailable");

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateRentalAsync_DatesOverlap_ThrowsInvalidOperationException()
        {
            var createDto = new CreateRentalDto
            {
                CarId = 1,
                RentalDate = new DateTime(2026, 12, 10),
                ReturnDate = new DateTime(2026, 12, 15)
            };

            var validCar = new Car { Id = 1, IsAvailable = true };
            
            var overlappingRentals = new List<Rental>
            {
                new Rental
                {
                    Status = "Active",
                    RentalDate = new DateTime(2026, 12, 12),
                    ReturnDate = new DateTime(2026, 12, 18)
                }
            };

            _unitOfWorkMock.Setup(u => u.Cars.GetByIdAsync(1)).ReturnsAsync(validCar);
            _unitOfWorkMock.Setup(u => u.Rentals.GetRentalsByCarIdAsync(1)).ReturnsAsync(overlappingRentals);

            Func<Task> act = async () => await _rentalService.CreateRentalAsync(createDto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Selected dates are not available");
        }

        #endregion

        #region UPDATE STATUS

        [Fact]
        public async Task CompleteRentalAsync_HappyPath_CompletesRental()
        {
            var rentalId = 5;
            var activeRental = new Rental { Id = rentalId, Status = "Active" };

            _unitOfWorkMock.Setup(u => u.Rentals.GetByIdAsync(rentalId))
                           .ReturnsAsync(activeRental);

            var result = await _rentalService.CompleteRentalAsync(rentalId);

            result.Should().BeTrue();
            activeRental.Status.Should().Be("Completed");
            _unitOfWorkMock.Verify(u => u.Rentals.Update(activeRental), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelRentalAsync_NotFound_ReturnsFalse()
        {
            var nonExistingRentalId = 999;
            _unitOfWorkMock.Setup(u => u.Rentals.GetByIdAsync(nonExistingRentalId))
                           .ReturnsAsync((Rental?)null);

            var result = await _rentalService.CancelRentalAsync(nonExistingRentalId);

            result.Should().BeFalse();
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        #endregion

    }
}
