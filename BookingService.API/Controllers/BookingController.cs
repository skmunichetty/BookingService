using BookingService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BookingService.Application.Exceptions;
using BookingService.Application.Models.Request;

namespace BookingService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService) 
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateBooking([FromBody]BookingModel model, IValidator<BookingModel> validator)
        {
            try
            {
                var validationResult = validator.Validate(model);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToDictionary());

                var response = _bookingService.CreateBooking(model);

                return Ok(response);
            }
            catch (BookingConflictException bcex)
            {
                return Conflict(bcex.Message);
            }            
        }
    }
}
