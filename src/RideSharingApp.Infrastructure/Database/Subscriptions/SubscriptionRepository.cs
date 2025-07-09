using Dapper;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Infrastructure.Database.Subscriptions;

public sealed class SubscriptionRepository : ISubscriptionRepository
{
    private readonly IConnection connection;

    public SubscriptionRepository(IConnection connection)
    {
        this.connection = connection;
    }

    public async Task<List<Subscription>> GetByUserIdAsync(Guid userId)
    {
        var subscriptions = await connection.DbConnection.QueryAsync<Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = userId });

        return subscriptions.ToList();
    }

    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        subscription.Id = Guid.CreateVersion7();

        await connection.DbConnection.ExecuteAsync("INSERT INTO Subscriptions (Id, UserId, StartDate) VALUES (@Id, @UserId, @StartDate)", subscription);

        return subscription;
    }
}
