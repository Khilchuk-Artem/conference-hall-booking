using ConferenceBooking.Application.Bookings.CreateBooking;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.UnitTests.TestData;

internal static class BookingTestData
{
    internal static CreateBookingCommand ValidCommand(Guid? hallId = null, params Guid[] serviceIds) => new()
    {
        ConferenceHallId = hallId ?? TestIds.HallId,
        StartTime = TestTimes.At(10),
        EndTime = TestTimes.At(12),
        AdditionalServiceIds = serviceIds.Length > 0 ? [.. serviceIds] : [TestIds.ServiceId]
    };

    internal static Booking Existing(Guid? id = null) => new()
    {
        Id = id ?? TestIds.BookingId,
        ConferenceHallId = TestIds.HallId,
        HallName = "Hall A",
        StartTime = TestTimes.At(10),
        EndTime = TestTimes.At(12),
        AdditionalServices = []
    };
}
