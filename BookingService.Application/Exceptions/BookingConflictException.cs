namespace BookingService.Application.Exceptions
{
    public class BookingConflictException: Exception
    {
        public BookingConflictException()
        {
        }

        public BookingConflictException(string message) : base(message)
        {
        }
    }
}
