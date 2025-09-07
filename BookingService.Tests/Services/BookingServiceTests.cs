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
        private readonly Application.Services.BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _bookingService = new Application.Services.BookingService(_mockBookingRepository.Object);
        }

        [Fact]
        public void Should_AddBooking_When_Bookings_In_That_Slot_Not_Filled()
        {
            var bookingTime = new TimeOnly(9, 30);
            var model = new BookingModel { BookingTime = "09:30", Name = "Sai Kiran" };

            _mockBookingRepository
                .Setup(r => r.GetBookingsInNextHour(bookingTime))
                .Returns(new List<Booking> 
                                { 
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("7c9bb30a-6f48-4f0e-9f8c-6b5fcd5b7c3e"),
                                        Name = "sai test",
                                        BookingTime =  new TimeOnly(9, 0)
                                    }, 
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("a2f1e5d7-9b8c-4d3c-91e4-0e7e3cf9a452"),
                                        Name = "kiran test",
                                        BookingTime =  new TimeOnly(9, 10)
                                    },
                                    new Booking()
                                    {
                                        BookingId = Guid.Parse("c5cb97fd-1fda-45e9-849d-d97bdfaa9fba"),
                                        Name = "naveen test",
                                        BookingTime =  new TimeOnly(9, 10)
                                    },
                                }); 

            _mockBookingRepository
                .Setup(r => r.Add(It.IsAny<Booking>()))
                .Returns(Guid.NewGuid());
                        
            var response = _bookingService.CreateBooking(model);
                        
            Assert.NotNull(response);
            Assert.NotEqual(Guid.Empty, response.BookingId);
            _mockBookingRepository.Verify(r => r.Add(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public void Should_Not_AddBooking_When_Bookings_Slots_Are_Filled()
        {            
            var bookingTime = new TimeOnly(9, 30);
            var model = new BookingModel { BookingTime = "09:30", Name = "Sai" };

            _mockBookingRepository
                .Setup(r => r.GetBookingsInNextHour(bookingTime))
                .Returns(new List<Booking> { new Booking(), new Booking(), new Booking(), new Booking() }); 
                        
            Assert.Throws<BookingConflictException>(() => _bookingService.CreateBooking(model));
            _mockBookingRepository.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void Should_Not_AddBooking_When_Duplicate_Name_Exists_In_Slot()
        {            
            var bookingTime = new TimeOnly(9, 30);
            var model = new BookingModel { BookingTime = "09:30", Name = "Sai" };

            _mockBookingRepository
                .Setup(r => r.GetBookingsInNextHour(bookingTime))
                .Returns(new List<Booking>
                {
                    new Booking { Name = "Sai", BookingTime = bookingTime } 
                });
                        
            Assert.Throws<BookingConflictException>(() => _bookingService.CreateBooking(model));
            _mockBookingRepository.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
        }

    }
}
