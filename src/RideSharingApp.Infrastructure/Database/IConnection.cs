using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
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

    public Connection(IOptions<ConectionString> options)
    {
        _conectionString = options.Value;
    }

    public IDbConnection DbConnection => CreateConnection();

    public IDbTransaction Transaction { get; set; }

    private IDbConnection CreateConnection()
    {
        if (DbConnection == null || DbConnection.State != ConnectionState.Open)
        {
            return new SqlConnection(_conectionString.RideConnection);
        }

        return DbConnection;
    }

    public void Dispose()
    {
        DbConnection?.Dispose();
    }
}