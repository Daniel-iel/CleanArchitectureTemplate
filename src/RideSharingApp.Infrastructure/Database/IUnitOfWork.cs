namespace RideSharingApp.Infrastructure.Database;
public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
}
