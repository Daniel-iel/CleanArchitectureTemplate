using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Rides.Metrics;
using RideSharingApp.SharedKernel.DomainEvents;
using System.Diagnostics.Metrics;

namespace RideSharingApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMeters()
            .AddScrutorScan()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }

    private static IServiceCollection AddScrutorScan(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
           .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
               .AsImplementedInterfaces()
               .WithScopedLifetime()
           .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
               .AsImplementedInterfaces()
               .WithScopedLifetime()
           .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
               .AsImplementedInterfaces()
               .WithScopedLifetime());

        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddMeters(this IServiceCollection services)
    {
        services.AddSingleton<Meter>(sp => new Meter("RideSharingAppApiMetrics"));

        services.AddScoped<INewRides, NewRides>();

        return services;
    }
}