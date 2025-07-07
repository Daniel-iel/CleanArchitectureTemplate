using RideSharingApp.Application.Abstractions.Messaging;

namespace RideSharingApp.Application.UseCases.Subscription.Create;

public sealed record CreateSubscriptionCommand(Guid UserId) : ICommand<SubscriptionResponse>;
