using Dapper;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.Domain.Login;
using RideSharingApp.Domain.Subscriptions;
using System.Data;

namespace RideSharingApp.Infrastructure.Database.Login;

public class UserRepository : IUserRepository
{
    private readonly IConnection _connection;

    public UserRepository(IConnection connection)
    {
        _connection = connection;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        const string query =
           """
               SELECT usu.*, sub.* FROM
               Users usu left join Subscriptions sub
                on usu.id = sub.user_id
               WHERE usu.Email = @Email;
           """;

        var command = new CommandDefinition(
           query,
           parameters: new { Email = email },
           commandType: CommandType.Text,
           cancellationToken: cancellationToken
        );

        var user = await _connection.DbConnection.QueryAsync<User, Subscription, User>(
            command,
            map: (user, subscription) =>
            {
                if (subscription != null)
                {
                    user.Subscriptions ??= new List<Subscription>();
                    user.Subscriptions.Add(subscription);
                }

                return user;
            },
            splitOn: "Id");

        return user.FirstOrDefault();
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query =
          """
               SELECT usu.*, sub.* FROM
               Users usu left join Subscriptions sub
                on usu.id = sub.user_id
               WHERE usu.Id = @Id;
           """;

        var command = new CommandDefinition(
            query,
            parameters: new { Id = id },
            commandType: CommandType.Text,
            cancellationToken: cancellationToken
        );

        var user = await _connection.DbConnection.QueryAsync<User, Subscription, User>(
           command,
           map: (user, subscription) =>
           {
               if (subscription != null)
               {
                   user.Subscriptions ??= new List<Subscription>();
                   user.Subscriptions.Add(subscription);
               }

               return user;
           },
           splitOn: "Id");

        return user.FirstOrDefault();
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();

        const string query =
            """
                INSERT INTO Users (Id, Email, PasswordHash) VALUES (@Id, @Email, @PasswordHash);
            """;

        var command = new CommandDefinition(
             query,
             parameters: user,
             commandType: CommandType.Text,
             cancellationToken: cancellationToken
        );

        return _connection.DbConnection.ExecuteAsync(command);
    }
}
