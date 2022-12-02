using Microsoft.EntityFrameworkCore;
using Shopping.API.Commons;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        private readonly ShoppingContext _context;
        public ReviewRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Review>, PaginationMetadata)> GetAllAsync(
            int? rating, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Reviews.Include(r => r.User) as IQueryable<Review>;

            if (rating != null)
            {
                colection = colection.Where(r => r.Rating == rating);
            }

            var totalItemCount = await colection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await colection.Skip(pageSize * (pageNumber - 1))  // Bỏ qua nội dung các trang trước đó
                                                    .Take(pageSize)                     // Lấy kích thước trang được yêu cầu hiện tại
                                                    .ToListAsync();                     // Tương tác với CSDL

            return (collectionToReturn, paginationMetadata);
        }

        public void Add(Account account, Product product, Review review)
        {
            product.Reviews.Add(review);
            account.Reviews.Add(review);
        }

        public void Delete(Account account, Product product, Review review)
        {
            Delete(review);
            product.Reviews.Remove(review);
            account.Reviews.Remove(review);
        }
    }
}
