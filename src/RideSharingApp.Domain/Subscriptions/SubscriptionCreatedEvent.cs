using RideSharingApp.SharedKernel.DomainEvents;

namespace RideSharingApp.Domain.Subscriptions;

public record SubscriptionCreatedEvent(Guid SubscriptionId, Guid UserId, DateTime CreatedAt) : IDomainEvent;