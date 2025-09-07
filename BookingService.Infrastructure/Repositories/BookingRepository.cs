using BookingService.Infrastructure.Entities;
using BookingService.Infrastructure.Interfaces;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings = new List<Booking>();
        public BookingRepository() { }

        public Guid Add(Booking booking)
        {
            booking.BookingId = Guid.NewGuid();
            _bookings.Add(booking);

            return booking.BookingId;
        }

        public IEnumerable<Booking> GetBookingsInNextHour(TimeOnly bookingTime)
        {
            var endTime = bookingTime.AddHours(1);

            return _bookings.Where(b => b.BookingTime >= bookingTime && b.BookingTime < endTime);
        }

    }
}
