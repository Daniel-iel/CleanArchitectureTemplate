using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Infrastructure.Database;
using RideSharingApp.Infrastructure.Database.Login;
using RideSharingApp.Infrastructure.Database.Rides;
using RideSharingApp.Infrastructure.Database.Settings;
using RideSharingApp.Infrastructure.Database.Subscriptions;

namespace RideSharingApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAppsettings()
            .AddMigration(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddAppsettings(this IServiceCollection services)
    {
        services
            .AddOptions<ConectionString>()
            .BindConfiguration(ConectionString.Key)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IConnection, Connection>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRideRepository, RideRepository>();

        return services;
    }

    private static IServiceCollection AddMigration(this IServiceCollection services, IConfiguration configuration)
    {
        // Executa a migration Flyway ao subir a aplicação
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            MigrateDatabase.RunFlywayMigration(connectionString);
        }
        else
        {
            Console.WriteLine("A string de conexão para o banco de dados não foi encontrada. Migration não executada.");
        }

        return services;
    }
}