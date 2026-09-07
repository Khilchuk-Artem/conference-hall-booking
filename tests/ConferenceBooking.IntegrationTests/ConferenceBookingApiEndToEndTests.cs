using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ConferenceBooking.IntegrationTests.Infrastructure;
using Xunit;

namespace ConferenceBooking.IntegrationTests;

public sealed class ConferenceBookingApiEndToEndTests : IClassFixture<ConferenceBookingApiFixture>
{
    private readonly HttpClient _client;

    public ConferenceBookingApiEndToEndTests(ConferenceBookingApiFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task ConferenceHallBookingWorkflow_CompletesThroughApi()
    {
        var createServiceResponse = await _client.PostAsJsonAsync("api/v1/AdditionalServices", new
        {
            name = "E2E Projector",
            price = 100m
        });

        Assert.Equal(HttpStatusCode.Created, createServiceResponse.StatusCode);
        var serviceId = await createServiceResponse.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, serviceId);
        Assert.NotNull(createServiceResponse.Headers.Location);
        Assert.Contains($"/api/v1/AdditionalServices/{serviceId}", createServiceResponse.Headers.Location!.ToString());

        var service = await GetAsync<AdditionalServiceResponse>($"api/v1/AdditionalServices/{serviceId}");
        Assert.Equal("E2E Projector", service.Name);
        Assert.Equal(100m, service.Price);

        var editServiceResponse = await _client.PutAsJsonAsync($"api/v1/AdditionalServices/{serviceId}", new
        {
            name = "E2E Projector Updated",
            price = 150m
        });

        Assert.Equal(HttpStatusCode.OK, editServiceResponse.StatusCode);
        var editedService = await editServiceResponse.Content.ReadFromJsonAsync<AdditionalServiceResponse>();
        Assert.NotNull(editedService);
        Assert.Equal(serviceId, editedService!.Id);
        Assert.Equal("E2E Projector Updated", editedService.Name);
        Assert.Equal(150m, editedService.Price);

        var services = await GetAsync<List<AdditionalServiceResponse>>(
            "api/v1/AdditionalServices?page=1&pageSize=10");
        Assert.Contains(services, item => item.Id == serviceId);

        var createHallResponse = await _client.PostAsJsonAsync("api/v1/ConferenceHalls", new
        {
            name = "E2E Hall",
            capacity = 80,
            rentRate = 2200m,
            additionalServiceIds = new[] { serviceId }
        });

        Assert.Equal(HttpStatusCode.Created, createHallResponse.StatusCode);
        var hallId = await createHallResponse.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, hallId);
        Assert.NotNull(createHallResponse.Headers.Location);
        Assert.Contains($"/api/v1/ConferenceHalls/{hallId}", createHallResponse.Headers.Location!.ToString());

        var hall = await GetAsync<ConferenceHallResponse>($"api/v1/ConferenceHalls/{hallId}");
        Assert.Equal("E2E Hall", hall.Name);
        Assert.Equal(80, hall.Capacity);
        Assert.Contains(hall.AdditionalServices, service => service.Id == serviceId);

        var editHallResponse = await _client.PutAsJsonAsync($"api/v1/ConferenceHalls/{hallId}", new
        {
            name = "E2E Hall Updated",
            capacity = 120,
            rentRate = 2400m,
            additionalServiceIds = new[] { serviceId }
        });

        Assert.Equal(HttpStatusCode.OK, editHallResponse.StatusCode);
        var editedHall = await editHallResponse.Content.ReadFromJsonAsync<ConferenceHallResponse>();
        Assert.NotNull(editedHall);
        Assert.Equal(hallId, editedHall!.Id);
        Assert.Equal("E2E Hall Updated", editedHall.Name);
        Assert.Equal(120, editedHall.Capacity);
        Assert.Equal(2400m, editedHall.RentRate);

        var availableHalls = await GetAsync<List<ConferenceHallResponse>>(
            $"api/v1/ConferenceHalls/available?capacity=100&startTime=2030-01-01T10:00:00%2B00:00&endTime=2030-01-01T12:00:00%2B00:00&page=1&pageSize=10");
        Assert.Contains(availableHalls, available => available.Id == hallId);

        var bookingRequest = new
        {
            conferenceHallId = hallId,
            startTime = "2030-01-01T10:00:00+00:00",
            endTime = "2030-01-01T12:00:00+00:00",
            additionalServiceIds = new[] { serviceId }
        };
        using var bookingRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/v1/Bookings")
        {
            Content = JsonContent.Create(bookingRequest)
        };
        bookingRequestMessage.Headers.Add("Idempotency-Key", $"e2e-booking-{Guid.NewGuid():N}");

        var createBookingResponse = await _client.SendAsync(bookingRequestMessage);
        Assert.Equal(HttpStatusCode.Created, createBookingResponse.StatusCode);
        Assert.NotNull(createBookingResponse.Headers.Location);
        var booking = await createBookingResponse.Content.ReadFromJsonAsync<BookingResponse>();
        Assert.NotNull(booking);
        Assert.NotEqual(Guid.Empty, booking!.Id);
        Assert.Equal(hallId, booking.ConferenceHallId);
        Assert.Equal("E2E Hall Updated", booking.HallName);
        Assert.Equal(4800m, booking.HallCost);
        Assert.Equal(300m, booking.TotalServicesCost);
        Assert.Equal(5100m, booking.TotalCost);
        Assert.Single(booking.AdditionalServices);

        var replayRequest = new HttpRequestMessage(HttpMethod.Post, "api/v1/Bookings")
        {
            Content = JsonContent.Create(bookingRequest)
        };
        replayRequest.Headers.Add("Idempotency-Key", bookingRequestMessage.Headers.GetValues("Idempotency-Key").Single());
        var replayResponse = await _client.SendAsync(replayRequest);
        Assert.Equal(HttpStatusCode.Created, replayResponse.StatusCode);
        var replayedBooking = await replayResponse.Content.ReadFromJsonAsync<BookingResponse>();
        Assert.Equal(booking.Id, replayedBooking!.Id);
        Assert.Equal(createBookingResponse.Headers.Location, replayResponse.Headers.Location);

        var bookingById = await GetAsync<BookingResponse>($"api/v1/Bookings/{booking.Id}");
        Assert.Equal(booking.Id, bookingById.Id);
        Assert.Equal(booking.TotalCost, bookingById.TotalCost);

        var report = await GetAsync<ReportSummaryResponse>(
            "api/v1/Reports/summary?from=2030-01-01T00:00:00%2B00:00&to=2030-01-02T00:00:00%2B00:00");
        Assert.Equal(1, report.BookingsCount);
        Assert.Equal(2d, report.TotalHours);
        Assert.Equal(5100m, report.TotalRevenue);
        var hallReport = Assert.Single(report.Halls, item => item.ConferenceHallId == hallId);
        Assert.Equal("E2E Hall Updated", hallReport.HallName);
        var serviceReport = Assert.Single(report.MostPopularAdditionalServices, item => item.AdditionalServiceId == serviceId);
        Assert.Equal(1, serviceReport.UsageCount);

        using var deleteServiceResponse = await _client.DeleteAsync($"api/v1/AdditionalServices/{serviceId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteServiceResponse.StatusCode);

        using var deletedServiceResponse = await _client.GetAsync($"api/v1/AdditionalServices/{serviceId}");
        Assert.Equal(HttpStatusCode.NotFound, deletedServiceResponse.StatusCode);
    }

    private async Task<T> GetAsync<T>(string requestUri)
    {
        using var response = await _client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        return result ?? throw new InvalidOperationException($"The API returned an empty response for {requestUri}.");
    }

    private class ConferenceHallResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal RentRate { get; set; }
        public List<AdditionalServiceResponse> AdditionalServices { get; set; } = [];
    }

    private class AdditionalServiceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    private class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid ConferenceHallId { get; set; }
        public string HallName { get; set; } = string.Empty;
        public decimal HallCost { get; set; }
        public decimal TotalServicesCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<BookingServiceResponse> AdditionalServices { get; set; } = [];
    }

    private class BookingServiceResponse
    {
        public Guid AdditionalServiceId { get; set; }
    }

    private class ReportSummaryResponse
    {
        public int BookingsCount { get; set; }
        public double TotalHours { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<HallReportResponse> Halls { get; set; } = [];
        public List<AdditionalServiceReportResponse> MostPopularAdditionalServices { get; set; } = [];
    }

    private class HallReportResponse
    {
        public Guid ConferenceHallId { get; set; }
        public string HallName { get; set; } = string.Empty;
    }

    private class AdditionalServiceReportResponse
    {
        public Guid AdditionalServiceId { get; set; }
        public int UsageCount { get; set; }
    }
}
