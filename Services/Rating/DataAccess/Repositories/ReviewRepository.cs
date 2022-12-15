using Microsoft.EntityFrameworkCore;
using Rating.Commons;
using Rating.DataAccess.DbContexts;
using Rating.DataAccess.Entities;

namespace Rating.DataAccess.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        private readonly ReviewContext _context;
        public ReviewRepository(ReviewContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Review>, PaginationMetadata)> GetAllAsync(
            int productId, int? ratingScore, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Reviews as IQueryable<Review>;

            colection = colection.Where(r => r.ProductId == productId);

            if (ratingScore != null)
            {
                colection = colection.Where(r => r.RatingScore == ratingScore);
            }

            var totalItemCount = await colection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await colection.Skip(pageSize * (pageNumber - 1))  // Bỏ qua nội dung các trang trước đó
                                                    .Take(pageSize)                     // Lấy kích thước trang được yêu cầu hiện tại
                                                    .ToListAsync();                     // Tương tác với CSDL

            return (collectionToReturn, paginationMetadata);
        }
    }
}
