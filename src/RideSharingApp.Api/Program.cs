using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version")
    );
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add services
var config = builder.Configuration;

//builder.Services.AddSingleton<UserRepository>(sp => new UserRepository(config.GetConnectionString("DefaultConnection") ?? ""));
//builder.Services.AddSingleton<SubscriptionRepository>(sp => new SubscriptionRepository(config.GetConnectionString("DefaultConnection") ?? ""));

//builder.Services.AddSingleton<RideRepository>(sp => new RideRepository(config.GetConnectionString("DefaultConnection") ?? ""));
// CQRS Handlers
//builder.Services.AddSingleton<RideSharingApp.Application.UseCases.Login.LoginCommandHandler>(sp =>
//    new RideSharingApp.Application.UseCases.Login.LoginCommandHandler(
//        sp.GetRequiredService<UserRepository>(),
//        config["Jwt:Key"] ?? "supersecretkey"
//    )
//);
//builder.Services.AddSingleton<CreateSubscriptionCommandHandler>(sp =>
//    new RideSharingApp.Application.UseCases.Subscription.CreateSubscriptionCommandHandler(
//        sp.GetRequiredService<SubscriptionRepository>(),
//    )
//);

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? "supersecretkey"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Register endpoint groups
RideSharingApp.Api.Endpoints.LoginEndpoints.MapLoginEndpoints(app, config);
RideSharingApp.Api.Endpoints.SubscriptionEndpoints.MapSubscriptionEndpoints(app);
RideSharingApp.Api.Endpoints.RideEndpoints.MapRideEndpoints(app);

await app.RunAsync();
