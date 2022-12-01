using Microsoft.EntityFrameworkCore;
using Shopping.API.Commons;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private readonly ShoppingContext _context;

        public CartRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Cart>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Carts as IQueryable<Cart>;

            colection = colection.Where(c => c.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                colection = colection.Where(p => p.Product.Name.Contains(searchQuery)
                                            || (p.Product.Brand != null && p.Product.Brand.Contains(searchQuery))
                                            || (p.Product.Category != null && p.Product.Category.Contains(searchQuery))
                                            || (p.Product.Description != null && p.Product.Description.Contains(searchQuery)));
            }

            var totalItemCount = await colection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await colection.Skip(pageSize * (pageNumber - 1))  // Bỏ qua nội dung các trang trước đó
                                                    .Take(pageSize)                     // Lấy kích thước trang được yêu cầu hiện tại
                                                    .ToListAsync();                     // Tương tác với CSDL

            return (collectionToReturn, paginationMetadata);
        }

        public void Add(User user, Product product, Cart cart)
        {
            user.Carts.Add(cart);
            product.Carts.Add(cart);
        }

        public void Delete(User user, Product product, Cart cart)
        {
            Delete(cart);
            user.Carts.Remove(cart);
            product.Carts.Remove(cart);
        }
    }
}
