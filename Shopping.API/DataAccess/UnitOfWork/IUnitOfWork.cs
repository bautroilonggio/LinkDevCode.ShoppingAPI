using Shopping.API.DataAccess.Repositories;

namespace Shopping.API.DataAccess
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IAccountRepository AccountRepository { get; }
        IReviewRepository ReviewRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task CommitAsync();
    }
}