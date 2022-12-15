using Microsoft.EntityFrameworkCore;
using Cart.Commons;
using Cart.DataAccess.DbContexts;

namespace Cart.DataAccess.Repositories
{
    public class CartRepository : RepositoryBase<Entities.Cart>, ICartRepository
    {
        private readonly CartContext _context;

        public CartRepository(CartContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Entities.Cart>, PaginationMetadata)> GetAllAsync(
            int userId, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Carts as IQueryable<Entities.Cart>;

            colection = colection.Where(c => c.UserId == userId);

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
