using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Login;

namespace RideSharingApp.Api.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app, IConfiguration config)
    {
        app.MapPost("/login", async (
            [FromBody] LoginCommand command,
            ICommandHandler<LoginCommand, LoginResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            if (result == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(result);
        });
    }
}
