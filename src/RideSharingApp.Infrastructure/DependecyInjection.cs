using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Infrastructure.Currency;
using RideSharingApp.Infrastructure.Database;
using RideSharingApp.Infrastructure.Database.Login;
using RideSharingApp.Infrastructure.Database.Rides;
using RideSharingApp.Infrastructure.Database.Settings;
using RideSharingApp.Infrastructure.Database.Subscriptions;
using RideSharingApp.Infrastructure.Migrations;
using System.Net;
using System.Threading.RateLimiting;

namespace RideSharingApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAppsettings()
            .AddRateLimiting()
            .AddQuotaApiHttp(configuration)
            .AddMigration(configuration)
            .AddRepositories();

        services.AddScoped<ICurrencyQuotationService, CurrencyQuotationService>();

        return services;
    }

    private static IServiceCollection AddQuotaApiHttp(this IServiceCollection services, IConfiguration configuration)
    {
        var currencyApiSetting = new CurrencyApiSetting();
        configuration.Bind(CurrencyApiSetting.Key, currencyApiSetting);

        services.AddHttpClient("CurrencyApi", client =>
        {
            client.BaseAddress = new Uri(currencyApiSetting.Url);
        })
        .AddResilienceHandler("custom", builder =>
        {
            builder
                .AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    UseJitter = true
                })
                .AddTimeout(TimeSpan.FromSeconds(10))
                .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    MinimumThroughput = 10,
                    BreakDuration = TimeSpan.FromSeconds(15)
                });
        });

        return services;
    }

    private static IServiceCollection AddAppsettings(this IServiceCollection services)
    {
        services
            .AddOptions<ConectionString>()
            .BindConfiguration(ConectionString.Key)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
           .AddOptions<CurrencyApiSetting>()
           .BindConfiguration(CurrencyApiSetting.Key)
           .ValidateDataAnnotations()
           .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }
                )
            );
            options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
        });

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