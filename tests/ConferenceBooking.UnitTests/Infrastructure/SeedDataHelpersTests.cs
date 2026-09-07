using ConferenceBooking.Infrastructure.Data.Helpers;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public class SeedDataHelpersTests
{
    [Fact]
    public void LoadAdditionalServices_ReturnsExpectedCatalog()
    {
        var services = SeedDataHelpers.LoadAdditionalServices();

        Assert.Equal(3, services.Count);
        Assert.Equal(["Projector", "Wi-Fi", "Sound"], services.Select(service => service.Name));
        Assert.Equal([500m, 300m, 700m], services.Select(service => service.Price));
        Assert.Equal(services.Count, services.Select(service => service.Id).Distinct().Count());
        Assert.All(services, service => Assert.False(service.IsDeleted));
    }

    [Fact]
    public void LoadConferenceHalls_ReturnsExpectedCatalog()
    {
        var halls = SeedDataHelpers.LoadConferenceHalls();

        Assert.Equal(3, halls.Count);
        Assert.Equal(["Hall A", "Hall B", "Hall C"], halls.Select(hall => hall.Name));
        Assert.Equal([50, 100, 30], halls.Select(hall => hall.Capacity));
        Assert.Equal([2000m, 3500m, 1500m], halls.Select(hall => hall.RentRate));
        Assert.Equal(halls.Count, halls.Select(hall => hall.Id).Distinct().Count());
        Assert.All(halls, hall => Assert.False(hall.IsDeleted));
    }

    [Fact]
    public void LoadConferenceHallAdditionalServices_ContainsEveryHallServicePairOnce()
    {
        var halls = SeedDataHelpers.LoadConferenceHalls();
        var services = SeedDataHelpers.LoadAdditionalServices();
        var pairs = SeedDataHelpers.LoadConferenceHallAdditionalServices();

        Assert.Equal(halls.Count * services.Count, pairs.Count);
        Assert.Equal(pairs.Count, pairs.Select(pair => pair.ToString()).Distinct().Count());
    }
}
