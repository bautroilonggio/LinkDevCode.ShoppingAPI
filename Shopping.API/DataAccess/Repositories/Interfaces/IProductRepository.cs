using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<(IEnumerable<Product>, PaginationMetadata)> GetAllAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<Product?> GetProductIncludeReviewsAsync(int productId);
        Task<Product?> GetProductIncludeCartsAsync(int productId);
        void UpdateTotalQuantityProduct(Product product, int quantityToOrder);
    }
}
