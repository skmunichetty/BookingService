# Booking Service API

This project implements a simple Booking API for InfoTrack’s settlement service.

It allows clients to reserve booking slots during business hours with the following rules:

- **Business Hours:** 9:00 AM – 5:00 PM (last booking 4:00 PM).
- **Booking Duration:** 1 hour 
- **Bookings:** Maximum 4 simultaneous settlements in a time slot(1 hr).

---

##  Technologies Used
- .NET 8 Web API
- FluentValidation
- Swagger 
- xUnit (Unit and Integration Tests)
- k6 (Load Testing)

---

## Running the Application

### 1. Prerequisites
- .NET 8 SDK

---

### 2. Run Locally
```bash
# Restore dependencies
cd ~/BookingService.API
dotnet restore

# Build the solution
cd ~/BookingService.API
dotnet build

# Run the API project
cd ~/BookingService.API
dotnet run --launch-profile https 

# Running Unit Tests
cd ~/BookingService.Tests
dotnet test

# Running Integration Tests
cd ~/BookingService.IntegrationTests
dotnet test

# Load Testing with k6
cd ~/BookingService.LoadTesting
k6 run booking-test.js
```

### 3. API Endpoint
Once the application is running, you can access Booking API at

- https://localhost:7018

Booking endpoint
- POST /api/booking/create

Example  Request 
```
POST https://localhost:7018/api/booking/create
Content-Type: application/json

{
  "bookingTime": "09:30",
  "name": "John Smith"
}
```

Example Success Response
```
{
  "bookingId": "d90f8c55-90a5-4537-a99d-c68242a6012b"
}
```

Swagger UI
- https://localhost:7018/swagger/index.html


