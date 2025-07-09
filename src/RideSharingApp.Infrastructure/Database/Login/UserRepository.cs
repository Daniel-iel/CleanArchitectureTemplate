using Dapper;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Login;

namespace RideSharingApp.Infrastructure.Database.Login;

public class UserRepository : IUserRepository
{
    private readonly IConnection connection;

    public UserRepository(IConnection connection)
    {
        this.connection = connection;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await connection.DbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
        if (user != null)
        {
            var subscriptions = await connection.DbConnection.QueryAsync<Domain.Subscriptions.Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = user.Id });
            user.Subscriptions = subscriptions.ToList();
        }

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await connection.DbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        if (user != null)
        {
            var subscriptions = await connection.DbConnection.QueryAsync<Domain.Subscriptions.Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = user.Id });
            user.Subscriptions = subscriptions.ToList();
        }
        return user;
    }

    public Task AddAsync(User user)
    {
        user.Id = Guid.NewGuid();
        return connection.DbConnection.ExecuteAsync("INSERT INTO Users (Id, Email, PasswordHash) VALUES (@Id, @Email, @PasswordHash)", user);
    }
}
