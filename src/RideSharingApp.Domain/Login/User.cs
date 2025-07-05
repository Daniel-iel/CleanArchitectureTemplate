using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Domain.Login;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<Subscription> Subscriptions { get; set; } = new();
}
