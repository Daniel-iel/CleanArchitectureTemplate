using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Login;

namespace RideSharingApp.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var mapGroup = endpointRouteBuilder
                .MapGroup("/user");

        mapGroup.MapPost("/login", async (
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

        return endpointRouteBuilder;
    }
}