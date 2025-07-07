using RideSharingApp.Application.Abstractions.Messaging;

namespace RideSharingApp.Application.UseCases.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
