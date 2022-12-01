using Microsoft.EntityFrameworkCore;
using Shopping.API.Commons;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly ShoppingContext _context;

        public ProductRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Product>, PaginationMetadata)> GetAllAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Products.Include(p => p.Reviews) as IQueryable<Product>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                colection = colection.Where(p => p.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                colection = colection.Where(p => p.Name.Contains(searchQuery)
                                            || (p.Brand != null && p.Brand.Contains(searchQuery))
                                            || (p.Category != null && p.Category.Contains(searchQuery))
                                            || (p.Description != null && p.Description.Contains(searchQuery)));
            }

            var totalItemCount = await colection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await colection.Skip(pageSize * (pageNumber - 1))  // Bỏ qua nội dung các trang trước đó
                                                    .Take(pageSize)                     // Lấy kích thước trang được yêu cầu hiện tại
                                                    .ToListAsync();                     // Tương tác với CSDL

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<Product?> GetProductIncludeReviewsAsync(int productId)
        {
            return await _context.Products.Where(p => p.Id == productId)
                        .Include(p => p.Reviews).ThenInclude(r => r.User).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetProductIncludeCartsAsync(int productId)
        {
            return await _context.Products.Where(p => p.Id == productId)
                        .Include(p => p.Carts).FirstOrDefaultAsync();
        }

        public void UpdateTotalQuantityProduct(Product product, int quantityToOrder)
        {
            product.TotalQuantity -= quantityToOrder;
        }
    }
}
