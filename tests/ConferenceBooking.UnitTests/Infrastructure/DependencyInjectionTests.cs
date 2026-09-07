using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Abstractions.Idempotency;
using ConferenceBooking.Infrastructure;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Data.Repositories;
using ConferenceBooking.Infrastructure.Idempotency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ConferenceBooking.UnitTests.Infrastructure;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_RegistersDatabaseAndRepositories()
    {
        IConfiguration configuration = new ConfigurationManager
        {
            ["ConnectionStrings:ConferenceBooking"] = "Host=localhost;Database=conference_booking;Username=postgres;Password=postgres"
        };
        var services = new ServiceCollection();

        services.AddInfrastructure(configuration);

        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(ConferenceBookingDbContext));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IRepository<>));
        Assert.Contains(services, descriptor => descriptor.ImplementationType == typeof(Repository<>));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IReportRepository));
        Assert.Contains(services, descriptor => descriptor.ImplementationType == typeof(ReportRepository));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IIdempotencyStore));
        Assert.Contains(services, descriptor => descriptor.ImplementationType == typeof(IdempotencyStore));

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Assert.IsType<ReportRepository>(scope.ServiceProvider.GetRequiredService<IReportRepository>());
        Assert.IsType<IdempotencyStore>(scope.ServiceProvider.GetRequiredService<IIdempotencyStore>());
    }
}
