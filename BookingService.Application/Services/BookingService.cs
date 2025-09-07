using BookingService.Application.Exceptions;
using BookingService.Application.Interfaces;
using BookingService.Application.Models.Request;
using BookingService.Application.Models.Response;
using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Interfaces;

namespace BookingService.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private static readonly object _lock = new object();

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public CreateBookingResponse CreateBooking(BookingModel model)
        {
            var bookingTime = TimeOnly.Parse(model.BookingTime);

            lock (_lock)
            {                
                var bookings = _bookingRepository.GetBookingsInNextHour(bookingTime).ToList();

                if (bookings.Count() >= 4)
                    throw new BookingConflictException("Booking slots are full. Choose a different time.");
                                
                var bookingEntity = MapToEntity(model);
                var bookingId = _bookingRepository.Add(bookingEntity);

                return new CreateBookingResponse { BookingId = bookingId };
            }
        }

        private Booking MapToEntity(BookingModel model)
        {
            return new Booking
            {
                BookingTime = TimeOnly.Parse(model.BookingTime),
                Name = model.Name,
            };
        }
    }
}
