using System.ComponentModel.DataAnnotations;

namespace BookingService.Application.Models.Request
{
    public class BookingModel
    {
        [Required]
        public string BookingTime { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
