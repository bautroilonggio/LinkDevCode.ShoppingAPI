using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IReviewRepository : IRepositoryBase<Review>
    {
        Task<(IEnumerable<Review>, PaginationMetadata)> GetAllAsync(
            int? rating, int pageNumber, int pageSize);
        void Add(User user, Product product, Review review);
        void Delete(User user, Product product, Review review);
    }
}
