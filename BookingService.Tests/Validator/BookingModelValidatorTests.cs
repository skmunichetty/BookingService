using BookingService.API.Validators;
using BookingService.Application.Models.Request;
using FluentValidation.TestHelper;

namespace BookingService.Tests.Validator
{
    public class BookingModelValidatorTests
    {
        private readonly BookingModelValidator _validator;

        public BookingModelValidatorTests()
        {
            _validator = new BookingModelValidator();
        }

        [Theory]
        [InlineData("Test User", true)]   
        [InlineData("Test", true)]        
        [InlineData("Test123", false)]    
        [InlineData("Test_user", false)]  
        [InlineData("Test@312", false)]   
        [InlineData("", false)]           
        [InlineData("                  ", false)]
        public void Validate_Name(string name, bool isValid)
        {
            var model = new BookingModel { BookingTime = "09:30", Name = name };
            var result = _validator.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(x => x.Name);
            else
                result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("09:00", true)]     
        [InlineData("12:30", true)]     
        [InlineData("16:00", true)]     
        [InlineData("8:59", false)]    
        [InlineData("16:01", false)]    
        [InlineData("16:59", false)]
        [InlineData("23:59", false)]
        [InlineData("not-a-time", false)] 
        [InlineData("25:99", false)]    
        [InlineData("00:101", false)]
        public void Validate_BookingTime(string time, bool isValid)
        {
            var model = new BookingModel { BookingTime = time, Name = "Sai" };
            var result = _validator.TestValidate(model);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(x => x.BookingTime);
            else
                result.ShouldHaveValidationErrorFor(x => x.BookingTime);
        }
    }
}
