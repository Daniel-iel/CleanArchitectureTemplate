using Microsoft.Extensions.Options;
using Npgsql;
using RideSharingApp.Infrastructure.Database.Settings;
using System.Data;

namespace RideSharingApp.Infrastructure.Database;

public interface IConnection
{
    public IDbConnection DbConnection { get; }

    public IDbTransaction Transaction { get; set; }
}

public sealed class Connection : IConnection, IDisposable
{
    private readonly ConectionString _conectionString;
    private NpgsqlConnection sqlConnection;

    public Connection(IOptions<ConectionString> options)
    {
        _conectionString = options.Value;
    }

    public IDbConnection DbConnection
    {
        get
        {
            sqlConnection = new NpgsqlConnection(_conectionString.RideConnection);
            sqlConnection.Open();

            return sqlConnection;
        }
    }

    public IDbTransaction Transaction { get; set; }

    public void Dispose()
    {
        sqlConnection?.Dispose();
    }
}