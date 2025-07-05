using Dapper;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Subscriptions;

namespace RideSharingApp.Infrastructure.Database.Subscriptions;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly string _connectionString;
    public SubscriptionRepository(string connectionString) => _connectionString = connectionString;

    public async Task<List<Subscription>> GetByUserIdAsync(Guid userId)
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        var subscriptions = await conn.QueryAsync<Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = userId });
        return subscriptions.ToList();
    }

    public async Task<Subscription> AddAsync(Subscription subscription)
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        subscription.Id = Guid.NewGuid();
        await conn.ExecuteAsync("INSERT INTO Subscriptions (Id, UserId, StartDate) VALUES (@Id, @UserId, @StartDate)", subscription);
        return subscription;
    }
}
