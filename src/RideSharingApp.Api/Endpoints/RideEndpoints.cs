using Microsoft.AspNetCore.Mvc;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.UseCases.Rides.GetRiders;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Api.Endpoints;

public static class RideEndpoints
{
    public static void MapRideEndpoints(this WebApplication app)
    {
        // Request a Ride (CQRS Command)
        app.MapPost("/rides/request", async (
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

        // Get Requested Rides (CQRS Query)
        app.MapGet("/rides/requests", async (
           GetRequestedRidesQuery query,
           IQueryHandler<GetRequestedRidesQuery, IEnumerable<RequestedRideResponse>> handler,
           CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
