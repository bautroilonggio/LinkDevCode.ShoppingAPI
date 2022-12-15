using Cart.DataAccess.Repositories;

namespace Cart.DataAccess
{
    public interface IUnitOfWork
    {
        ICartRepository CartRepository { get; }
        Task CommitAsync();
    }
}