using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Application.Abstractions.Idempotency;
using ConferenceBooking.Infrastructure.Data;
using ConferenceBooking.Infrastructure.Data.Repositories;
using ConferenceBooking.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConferenceBooking");

        services.AddDbContext<ConferenceBookingDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();

        return services;
    }
}
