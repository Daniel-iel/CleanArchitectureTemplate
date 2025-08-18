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

    public async Task<List<Subscription>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string query =
            """
                SELECT * FROM Subscriptions WHERE user_id = @UserId";
            """;

        var parameters = new
        {
            UserId = userId
        };

        var command = new CommandDefinition(
            commandText: query,
            parameters: parameters,
            cancellationToken: cancellationToken
        );

        var subscriptions = await connection.DbConnection.QueryAsync<Subscription>(command);

        return subscriptions.ToList();
    }

    public async Task<Subscription> AddAsync(Subscription subscription, CancellationToken cancellationToken)
    {
        subscription.Id = Guid.CreateVersion7();

        const string query =
            """
                INSERT INTO Subscriptions (Id, UserId, StartDate)
                VALUES (@Id, @UserId, @StartDate);
            """;

        var command = new CommandDefinition(
            commandText: query,
            parameters: subscription,
            cancellationToken: cancellationToken,
            transaction: connection.Transaction
        );

        await connection.DbConnection.ExecuteAsync("", subscription);

        return subscription;
    }
}
