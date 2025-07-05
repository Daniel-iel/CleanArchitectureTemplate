using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    // Removido: List<Subscription> Subscriptions. Use o SubscriptionRepository para buscar subscriptions.
}
