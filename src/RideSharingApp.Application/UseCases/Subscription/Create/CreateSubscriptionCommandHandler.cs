using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Subscriptions;
using RideSharingApp.SharedKernel.DispacherEvent;
using RideSharingApp.SharedKernel.Results;
using SubscriptionEntity = RideSharingApp.Domain.Subscriptions.Subscription;

namespace RideSharingApp.Application.UseCases.Subscription.Create;

public sealed class CreateSubscriptionCommandHandler : ICommandHandler<CreateSubscriptionCommand, SubscriptionResponse>
{
    private readonly ISubscriptionRepository _subRepo;
    private readonly IEventPublisher eventPublisher;

    public CreateSubscriptionCommandHandler(ISubscriptionRepository subRepo, IEventPublisher eventPublisher)
    {
        _subRepo = subRepo;
        this.eventPublisher = eventPublisher;
    }

    public async Task<Result<SubscriptionResponse>> HandleAsync(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = new SubscriptionEntity { UserId = command.UserId, StartDate = DateTime.UtcNow };
        var created = await _subRepo.AddAsync(subscription);

        if (created == null)
        {
            return Result.Failure<SubscriptionResponse>(Error.Problem("Subscription.CreationFailed", "Falha ao criar assinatura."));
        }

        await eventPublisher.DispatchAsync(new SubscriptionCreatedEvent(created.Id, created.UserId, DateTime.UtcNow), cancellationToken);

        return Result.Success(new SubscriptionResponse(created.Id));
    }
}
