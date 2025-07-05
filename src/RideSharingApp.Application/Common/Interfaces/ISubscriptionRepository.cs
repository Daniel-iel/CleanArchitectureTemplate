using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Application.Common.Interfaces;

public interface ISubscriptionRepository
{
    Task<List<Subscription>> GetByUserIdAsync(Guid userId);
    Task<Subscription> AddAsync(Subscription subscription);
}
