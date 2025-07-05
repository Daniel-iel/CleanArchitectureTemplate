using RideSharingApp.Application.Common.DispacherEvent;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Subscriptions;

using SubscriptionEntity = RideSharingApp.Domain.Subscriptions.Subscription;

namespace RideSharingApp.Application.UseCases.Subscription.Create;

public class CreateSubscriptionCommandHandler
{
    private readonly ISubscriptionRepository _subRepo;
    private readonly EventPublisher eventPublisher;

    public CreateSubscriptionCommandHandler(ISubscriptionRepository subRepo, EventPublisher eventPublisher)
    {
        _subRepo = subRepo;
        this.eventPublisher = eventPublisher;
    }

    public async Task<SubscriptionResponse> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new SubscriptionEntity { UserId = command.UserId, StartDate = DateTime.UtcNow };
        var created = await _subRepo.AddAsync(subscription);

        await eventPublisher.PublishAsync(new SubscriptionCreatedEvent(created.Id, created.UserId));

        return new SubscriptionResponse(created.Id);
    }
}
