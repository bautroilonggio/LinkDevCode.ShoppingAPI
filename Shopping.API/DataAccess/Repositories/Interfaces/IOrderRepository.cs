using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<(IEnumerable<Order>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize);
        void Add(User user, Order order);
        void Delete(User user, Order order);
    }
}
