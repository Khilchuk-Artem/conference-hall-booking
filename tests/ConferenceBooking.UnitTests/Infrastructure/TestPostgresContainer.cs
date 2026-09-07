using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public sealed class TestPostgresContainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("conference_booking_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    internal ConferenceBookingDbContext CreateContext() => new(
        new DbContextOptionsBuilder<ConferenceBookingDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options);

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();
}
