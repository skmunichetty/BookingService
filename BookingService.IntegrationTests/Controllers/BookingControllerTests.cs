using System.Net.Http.Json;
using BookingService.API;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using BookingService.Application.Models.Request;
using BookingService.Application.Models.Response;

namespace BookingService.IntegrationTests.Controllers
{
    public class BookingControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BookingControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:7018") 
            });
        }

        [Fact]
        public async Task CreateBooking_Should_Return_200_When_Slot_Available()
        {
            var booking = new BookingModel
            {
                BookingTime = "09:30",
                Name = "Test User"
            };
         
            var response = await _client.PostAsJsonAsync("/api/booking/create", booking);
                        
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var bookingResponse = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();
            Assert.NotNull(bookingResponse);
            bookingResponse.BookingId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task CreateBooking_Should_Return_409_When_SlotIsFull()
        {
            var booking = new BookingModel
            {
                BookingTime = "10:00",
                Name = "Test User"
            };

            for (int i = 0; i < 4; i++)
            {
                var response = await _client.PostAsJsonAsync("/api/booking/create", booking);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            // 5th call to the CreateBooking should fail
            var conflictResponse = await _client.PostAsJsonAsync("/api/booking/create", booking);

            conflictResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateBooking_Should_Return_400_When_InvalidName()
        {
            var booking = new BookingModel
            {
                BookingTime = "09:30",
                Name = "Test@123" 
            };

            var response = await _client.PostAsJsonAsync("/api/booking/create", booking);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateBooking_Should_Return_400_When_Invalid_BookingTime()
        {
            var booking = new BookingModel
            {
                BookingTime = "00:30",
                Name = "Test@123"
            };

            var response = await _client.PostAsJsonAsync("/api/booking/create", booking);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
