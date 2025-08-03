using RideSharingApp.Api;
using RideSharingApp.Application;
using RideSharingApp.Infrastructure;
using RideSharingApp.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .AddApi(config)
    .AddApplication()
    .AddInfrastructure(config)
    .AddSharedKernel();

builder.HostSerilog();

var app = builder.Build();
app.UseApi();

await app.RunAsync();
