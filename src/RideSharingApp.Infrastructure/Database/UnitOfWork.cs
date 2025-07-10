using RideSharingApp.Application.Common.Interfaces;

namespace RideSharingApp.Infrastructure.Database;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private IConnection _connection;

    public UnitOfWork(IConnection connection)
    {
        _connection = connection;
    }

    public void BeginTransaction()
    {
        _connection.Transaction = _connection.DbConnection.BeginTransaction();
    }

    public void Commit()
    {
        _connection.Transaction.Commit();
    }

    public void Rollback()
    {
        _connection.Transaction.Rollback();
    }

    public void Dispose()
    {
        _connection.Transaction?.Dispose();
    }
}
