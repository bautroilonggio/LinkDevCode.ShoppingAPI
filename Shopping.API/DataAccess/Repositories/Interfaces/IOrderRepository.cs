using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;
using System.Linq.Expressions;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<(IEnumerable<Order>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize);
        Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> where);
        void Add(User user, Order order);
        void Delete(User user, Order order);
    }
}
