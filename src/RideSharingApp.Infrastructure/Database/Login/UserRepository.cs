using Dapper;
using RideSharingApp.Domain.Login;
using RideSharingApp.Application.Common.Interfaces;

namespace RideSharingApp.Infrastructure.Database.Login;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    public UserRepository(string connectionString) => _connectionString = connectionString;

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        var user = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
        if (user != null)
        {
            var subscriptions = await conn.QueryAsync<Domain.Subscriptions.Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = user.Id });
            user.Subscriptions = subscriptions.ToList();
        }
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        var user = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        if (user != null)
        {
            var subscriptions = await conn.QueryAsync<Domain.Subscriptions.Subscription>("SELECT * FROM Subscriptions WHERE UserId = @UserId", new { UserId = user.Id });
            user.Subscriptions = subscriptions.ToList();
        }
        return user;
    }

    public async Task AddAsync(User user)
    {
        using var conn = new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
        user.Id = Guid.NewGuid();
        await conn.ExecuteAsync("INSERT INTO Users (Id, Email, PasswordHash) VALUES (@Id, @Email, @PasswordHash)", user);
    }
}
