using Cart.Commons;

namespace Cart.DataAccess.Repositories
{
    public interface ICartRepository : IRepositoryBase<Entities.Cart>
    {
        Task<(IEnumerable<Entities.Cart>, PaginationMetadata)> GetAllAsync(
            int userId, int pageNumber, int pageSize);
    }
}
