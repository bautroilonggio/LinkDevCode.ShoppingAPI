using Ordering.Commons;
using Ordering.DataAccess.Entities;
using System.Linq.Expressions;

namespace Ordering.DataAccess.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<(IEnumerable<Order>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize);
        Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> where);
    }
}
