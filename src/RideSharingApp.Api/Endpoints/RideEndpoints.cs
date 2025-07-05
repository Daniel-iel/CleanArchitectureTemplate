using FluentValidation;
using RideSharingApp.Application.UseCases.Rides.GetRiders;
using RideSharingApp.Application.UseCases.Rides.RequestRiders;

namespace RideSharingApp.Api.Endpoints;

public static class RideEndpoints
{
    public static void MapRideEndpoints(this WebApplication app)
    {
        // Request a Ride (CQRS Command)
        app.MapPost("/rides/request", async (
            RequestRideCommand command,
            RequestRideCommandHandler handler,
            IValidator<RequestRideCommand> validator) =>
        {
            var dto = new RequestRideCommand(command.PassengerId, command.PickupLocation, command.DropoffLocation);
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            try
            {
                var result = await handler.Handle(command);
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
           GetRequestedRidesQueryHandler handler) =>
        {
            var result = await handler.HandleAsync(query);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
