using FluentValidation;
using RideSharingApp.Application.UseCases.Subscription.Create;

namespace RideSharingApp.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        app.MapPost("/subscriptions", async (
            CreateSubscriptionCommand command,
            CreateSubscriptionCommandHandler handler,
            IValidator<CreateSubscriptionCommand> validator) =>
        {
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var result = await handler.Handle(command);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}
