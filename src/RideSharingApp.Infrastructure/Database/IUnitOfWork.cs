using System.Threading.Tasks;

namespace RideSharingApp.Infrastructure.Database;

using System.Data;
public interface IUnitOfWork
{
    Task BeginAsync();
    Task CommitAsync();
    Task RollbackAsync();
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
}
