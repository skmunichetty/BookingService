using BookingService.Infrastructure.Entities;

namespace BookingService.Infrastructure.Interfaces
{
    public interface IBookingRepository
    {
        Guid Add(Booking booking);
        IEnumerable<Booking> GetBookingsInNextHour(TimeOnly bookingTime);
    }
}
