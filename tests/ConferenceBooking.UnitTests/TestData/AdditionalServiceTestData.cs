using ConferenceBooking.Application.AdditionalServices.CreateAdditionalService;
using ConferenceBooking.Application.AdditionalServices.EditAdditionalService;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.UnitTests.TestData;

internal static class AdditionalServiceTestData
{
    internal static AdditionalService Projector() => new()
    {
        Id = TestIds.ServiceId,
        Name = "Projector",
        Price = 100m
    };

    internal static CreateAdditionalServiceCommand ValidCreateCommand() => new()
    {
        Name = "Projector",
        Price = 100m
    };

    internal static EditAdditionalServiceCommand ValidEditCommand() => new()
    {
        Id = TestIds.ServiceId,
        Name = "Updated Projector",
        Price = 150m
    };
}
