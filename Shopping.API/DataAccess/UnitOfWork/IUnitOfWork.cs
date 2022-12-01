using Shopping.API.DataAccess.Repositories;

namespace Shopping.API.DataAccess
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        IReviewRepository ReviewRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task CommitAsync();
    }
}