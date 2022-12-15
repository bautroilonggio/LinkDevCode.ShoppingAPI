using Catalog.Commons;
using Catalog.DataAccess.Entities;

namespace Catalog.DataAccess.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<(IEnumerable<Product>, PaginationMetadata)> GetAllAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        void UpdateTotalQuantityProduct(Product product, int quantityToOrder);
    }
}
