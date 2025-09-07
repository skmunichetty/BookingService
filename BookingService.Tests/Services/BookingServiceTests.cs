using BookingService.Application.Exceptions;
using BookingService.Application.Models.Request;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Interfaces;
using Moq;

namespace BookingService.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepository;
        private readonly Application.Services.BookingService _service;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _service = new Application.Services.BookingService(_mockBookingRepository.Object);
        }

        [Fact]
        public void Should_AddBooking_When_Bookings_In_That_Slot_Not_Filled()
        {
            // Arrange
            var bookingTime = new TimeOnly(9, 30);
            var model = new BookingModel { BookingTime = "09:30", Name = "Sai Kiran" };

            _mockBookingRepository
                .Setup(r => r.GetBookingsInNextHour(bookingTime))
                .Returns(new List<Booking> 
                                { 
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("c5cb97fd-1fda-45e9-849d-d97bdfaa9fbf"),
                                        Name = "test",
                                        BookingTime =  new TimeOnly(9, 0)
                                    }, 
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("c5cb97fd-1fda-45e9-849d-d97bdfaa9fbg"),
                                        Name = "test",
                                        BookingTime =  new TimeOnly(9, 10)
                                    },
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("c5cb97fd-1fda-45e9-849d-d97bdfaa9fbh"),
                                        Name = "test",
                                        BookingTime =  new TimeOnly(9, 10)
                                    },
                                }); 

            _mockBookingRepository
                .Setup(r => r.Add(It.IsAny<Booking>()))
                .Returns(Guid.NewGuid());

            // Act
            var response = _service.CreateBooking(model);

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.BookingId);
            _mockBookingRepository.Verify(r => r.Add(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public void Should_Not_AddBooking_When_Bookings_Slots_Are_Filled()
        {
            // Arrange
            var bookingTime = new TimeOnly(9, 30);
            var model = new BookingModel { BookingTime = "09:30", Name = "Sai" };

            _mockBookingRepository
                .Setup(r => r.GetBookingsInNextHour(bookingTime))
                .Returns(new List<Booking> { new Booking(), new Booking(), new Booking(), new Booking() }); 

            // Act & Assert
            Assert.Throws<BookingConflictException>(() => _service.CreateBooking(model));
            _mockBookingRepository.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
        }        
    }
}
