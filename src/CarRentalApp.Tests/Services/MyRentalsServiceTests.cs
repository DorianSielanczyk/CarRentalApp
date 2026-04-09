using CarRentalApp.Application.Services;
using CarRentalApp.Domain.Entities;
using CarRentalApp.Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace CarRentalApp.Tests.Services
{
    public class MyRentalsServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IWebHostEnvironment> _environmentMock;
        private readonly MyRentalsService _myRentalsService;

        public MyRentalsServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _environmentMock = new Mock<IWebHostEnvironment>();
            
            _myRentalsService = new MyRentalsService(_unitOfWorkMock.Object, _environmentMock.Object);
        }

        [Fact]
        public async Task CancelReservationAsync_WhenMoreThan7DaysLeft_CancelsSuccessfully()
        {
            var rental = new Rental
            {
                Id = 1,
                Status = "Active",
                RentalDate = DateTime.Today.AddDays(10) 
            };

            _unitOfWorkMock.Setup(u => u.Rentals.GetByIdAsync(1)).ReturnsAsync(rental);

            var result = await _myRentalsService.CancelReservationAsync(1);

            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.SuccessMessage.Should().Be("Reservation cancelled.");
            rental.Status.Should().Be("Cancelled");

            _unitOfWorkMock.Verify(u => u.Rentals.Update(rental), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelReservationAsync_WhenLessThan7DaysLeft_ReturnsError()
        {
            var rental = new Rental
            {
                Id = 1,
                Status = "Active",
                RentalDate = DateTime.Today.AddDays(5) 
            };

            _unitOfWorkMock.Setup(u => u.Rentals.GetByIdAsync(1)).ReturnsAsync(rental);

            var result = await _myRentalsService.CancelReservationAsync(1);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("You can cancel only if booking starts in more than 7 days.");
            rental.Status.Should().Be("Active");

            _unitOfWorkMock.Verify(u => u.Rentals.Update(It.IsAny<Rental>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}