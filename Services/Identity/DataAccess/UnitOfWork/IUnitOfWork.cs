using Identity.DataAccess.Repositories;

namespace Identity.DataAccess
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        Task CommitAsync();
    }
}