using FluentValidation;
using RideSharingApp.Application.UseCases.Subscription.Create;

namespace RideSharingApp.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static IEndpointRouteBuilder MapSubscriptionsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var mapGroup = endpointRouteBuilder
                .MapGroup("/subscriptions");

        mapGroup.MapPost("/", async (
          CreateSubscriptionCommand command,
          CreateSubscriptionCommandHandler handler,
          IValidator<CreateSubscriptionCommand> validator,
          CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var result = await handler.HandleAsync(command, cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();

        return endpointRouteBuilder;
    }
}
