using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace RideSharingApp.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly string _connectionString;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task BeginAsync()
    {
        _connection = new SqlConnection(_connectionString);
        await ((SqlConnection)_connection).OpenAsync();
        _transaction = _connection.BeginTransaction();
    }

    public Task CommitAsync()
    {
        _transaction?.Commit();
        Dispose();
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        _transaction?.Rollback();
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
        _connection?.Dispose();
        _connection = null;
    }

    public IDbConnection Connection => _connection ?? throw new InvalidOperationException("UnitOfWork connection is not initialized.");
    public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("UnitOfWork transaction is not initialized.");
}
