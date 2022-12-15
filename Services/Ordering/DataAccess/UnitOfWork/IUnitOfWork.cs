using Ordering.DataAccess.Repositories;

namespace Ordering.DataAccess
{
    public interface IUnitOfWork
    {
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        Task CommitAsync();
    }
}