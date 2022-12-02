using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public interface ICartRepository : IRepositoryBase<Cart>
    {
        Task<(IEnumerable<Cart>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize);
        void Add(Account user, Product product, Cart cart);
        void Delete(Account user, Product product, Cart cart);
    }
}
