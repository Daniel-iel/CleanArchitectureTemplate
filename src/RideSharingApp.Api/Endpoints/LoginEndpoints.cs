using FluentValidation;
using RideSharingApp.Application.UseCases.Login;

namespace RideSharingApp.Api.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app, IConfiguration config)
    {
        app.MapPost("/login", async (
            LoginCommand command,
            LoginCommandHandler handler,
            IValidator<LoginCommand> validator) =>
        {
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var result = await handler.Handle(command);
            if (result == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(result);
        });
    }
}
