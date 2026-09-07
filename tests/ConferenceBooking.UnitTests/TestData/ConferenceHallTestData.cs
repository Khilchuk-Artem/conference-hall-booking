using ConferenceBooking.Application.ConferenceHalls.CreateConferenceHall;
using ConferenceBooking.Application.ConferenceHalls.EditConferenceHall;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.UnitTests.TestData;

internal static class ConferenceHallTestData
{
    internal static CreateConferenceHallCommand ValidCommand(params Guid[] serviceIds) => new()
    {
        Name = "Hall A",
        Capacity = 100,
        RentRate = 2000m,
        AdditionalServiceIds = serviceIds.Length > 0 ? [.. serviceIds] : [TestIds.ServiceId]
    };

    internal static EditConferenceHallCommand ValidEditCommand(params Guid[] serviceIds) => new()
    {
        Id = TestIds.HallId,
        Name = "Hall A",
        Capacity = 100,
        RentRate = 2000m,
        AdditionalServiceIds = serviceIds.Length > 0 ? [.. serviceIds] : [TestIds.ServiceId]
    };

    internal static ConferenceHall Hall(bool includeWiFi = true)
    {
        var services = new List<AdditionalService> { ProjectorService() };
        if (includeWiFi) services.Add(WiFiService());

        return new ConferenceHall
        {
            Id = TestIds.HallId,
            Name = "Hall A",
            Capacity = 100,
            RentRate = 2000m,
            AdditionalServices = services,
            Bookings = []
        };
    }

    internal static AdditionalService ProjectorService() => new()
    {
        Id = TestIds.ServiceId,
        Name = "Projector",
        Price = 100m
    };

    internal static AdditionalService WiFiService() => new()
    {
        Id = TestIds.SecondServiceId,
        Name = "Wi-Fi",
        Price = 250m
    };
}
