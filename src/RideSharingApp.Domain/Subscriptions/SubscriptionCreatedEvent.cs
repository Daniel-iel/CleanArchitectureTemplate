namespace RideSharingApp.Domain.Subscriptions;

public class SubscriptionCreatedEvent
{
    public Guid SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public SubscriptionCreatedEvent(Guid subscriptionId, Guid userId)
    {
        SubscriptionId = subscriptionId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
}
