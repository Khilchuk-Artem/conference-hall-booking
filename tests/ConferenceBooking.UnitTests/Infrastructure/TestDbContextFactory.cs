using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.UnitTests.Infrastructure;

internal static class TestDbContextFactory
{
    internal static ConferenceBookingDbContext Create() => new(
        new DbContextOptionsBuilder<ConferenceBookingDbContext>()
            .UseNpgsql("Host=localhost;Database=conference_booking;Username=postgres;Password=postgres")
            .Options);
}
