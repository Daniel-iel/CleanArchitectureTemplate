using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RideSharingApp.Api.Middlewares;
using Serilog;


namespace RideSharingApp.Api;

public static class DependencyInjection
{
    public static WebApplication UseApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app
            .UseMiddleware<SecurityHeadersMiddleware>()
            .UseMiddleware<CorrelationIdMiddleware>()
            .UseMiddleware<GlobalExceptionMiddleware>()
            .UseMiddleware<IdempotencyMiddleware>();

        return app;
    }

    public static WebApplicationBuilder HostSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
        });

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService("RideSharingAppApi", serviceVersion: "1.0.0");

        // ---------- LOGGING ----------
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging
                .SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("http://otel-collector:4317");
                });
        });

        return builder;
    }

    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllers();

        services
            .AddAuth(configuration)
            .AddVersioning()
            .AddSwaggerGen()
            .AddCompression()
            .UseSerilog(configuration)
            .AddOpenTel();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Migrar para class
        var keycloakUrl = configuration["Keycloak:Url"] ?? "http://localhost:8080";
        var realm = configuration["Keycloak:Realm"] ?? "templateAuth";
        var audience = configuration["Keycloak:ClientId"] ?? "ridesharing-api";

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"{keycloakUrl}/realms/{realm}";
                options.Audience = audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version")
            );
            options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
        })
       .AddMvc();

        return services;
    }

    private static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Ride Sharing API",
                Version = "v1",
                Description = "API for Ride Sharing Application"
            });
        });
        return services;
    }

    private static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services
            .AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            })
            .Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal
;
            });

        return services;
    }

    private static IServiceCollection UseSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }

    private static IServiceCollection AddOpenTel(this IServiceCollection services)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService("RideSharingAppApi", serviceVersion: "1.0.0");

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService(nameof(RideSharingApp)))
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4318");
                    });
            });

        // ---------- MÉTRICAS ----------
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter("RideSharingAppApiMetrics")
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://otel-collector:4317");
                    });
            });

        // ---------- TRACING ----------
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://otel-collector:4317");
                    });
            });

        return services;
    }
}

//internal class NewRides : INewRides
//{
//    private readonly Counter<long> _ordersCounter;

//    public NewRides(IMeterProvider meterProvider)
//    {
//        // Cria um meter via abstração do OpenTelemetry
//        var meter = meterProvider.GetMeter("MyAppMetrics");
//        _ordersCounter = meter.CreateCounter<long>(
//            "orders_total",
//            description: "Quantidade total de pedidos criados"
//        );
//    }

//    public void Increment()
//    {
//        _ordersCounter.Add(1);
//    }
//}