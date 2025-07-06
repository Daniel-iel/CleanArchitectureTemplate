using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Rides.GetRiders;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Api.Endpoints;

public static class RideEndpoints
{
    public static IEndpointRouteBuilder MapRideEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var mapGroup = endpointRouteBuilder
               .MapGroup("/rides");

        mapGroup.MapPost("/", async (
            [FromBody] RequestRideCommand command,
            ICommandHandler<RequestRideCommand, RequestRideResponse> handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).RequireAuthorization();

        mapGroup.MapGet("/requests", async (
           GetRequestedRidesQuery query,
           IQueryHandler<GetRequestedRidesQuery, IEnumerable<RequestedRideResponse>> handler,
           CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();

        return endpointRouteBuilder;
    }
}
