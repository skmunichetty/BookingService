using BookingService.Application.Models.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BookingService.API.Validators
{
    public class BookingModelValidator: AbstractValidator<BookingModel>
    {
        public BookingModelValidator()
        {
            RuleFor(x => x.BookingTime)
                .NotEmpty().WithMessage("Booking time is required")
                .Must(ValidTime)
                .WithMessage("Booking time is not a valid time.")
                .MaximumLength(5).WithMessage("Booking time must be in HH:mm format.")
                .Must(IsBookingTimeWithinWorkingHours)
                .WithMessage("Booking time must be between 09:00 and 16:00");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed more than 100 characters.")
                .Must(ShouldOnlyContainLettersndSpaces).WithMessage("Name is not valid.");
        }

        private bool ShouldOnlyContainLettersndSpaces(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return Regex.IsMatch(name, @"^[A-Za-z ]+$");
        }

        private bool ValidTime(string time)
        {
            if (!TimeOnly.TryParse(time, out var t))
                return false;

            return true;
        }

        private bool IsBookingTimeWithinWorkingHours(string bookingTime)
        {
            if (!TimeOnly.TryParse(bookingTime, out var bt))
                return false;

            var startTime = new TimeOnly(9, 0);
            var endTime = new TimeOnly(16, 0); 

            return bt >= startTime && bt <= endTime;
        }
    }
}
