using BookingService.Application.Models.Request;
using BookingService.Application.Models.Response;

namespace BookingService.Application.Interfaces
{
    public interface IBookingService
    {
        CreateBookingResponse CreateBooking(BookingModel model);
    }
}
