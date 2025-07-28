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

var app = builder.Build();

app.UseApi();
//app.UseSwagger();
//app.UseSwaggerUI();
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

await app.RunAsync();
