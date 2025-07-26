using RideSharingApp.SharedKernel.DomainEvents;

namespace RideSharingApp.SharedKernel.DispacherEvent;

public interface IEventPublisher
{
    Task DispatchAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
}
