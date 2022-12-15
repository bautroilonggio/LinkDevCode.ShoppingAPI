using Catalog.Commons;
using Catalog.DataAccess.Models;
using Catalog.DataAccess.Models.Product;

namespace Catalog.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductDto>, PaginationMetadata)> GetProductsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<ProductDto?> GetProductAsync(int productId);
        Task<ProductDto> CreateProductAsync(ProductForCreateDto product);
        Task<bool> UpdateProductAsync(int productId, ProductForUpdateDto product);
        Task<bool> DeleteProductAsync(int productId);
    }
}