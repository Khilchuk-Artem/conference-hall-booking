using System.Reflection;
using ConferenceBooking.Application.Behaviors;
using ConferenceBooking.Domain.Pricing;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton<RentPriceCalculator>();

        return services;
    }
}
