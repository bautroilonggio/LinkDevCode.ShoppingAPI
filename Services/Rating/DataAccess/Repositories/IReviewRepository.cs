using Rating.Commons;
using Rating.DataAccess.Entities;

namespace Rating.DataAccess.Repositories
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        Task<(IEnumerable<Review>, PaginationMetadata)> GetAllAsync(
            int productId, int? ratingScore, int pageNumber, int pageSize);
    }
}
