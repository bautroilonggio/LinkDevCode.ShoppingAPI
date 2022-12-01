using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;
using Shopping.API.DataAccess.Models.Product;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductDto>, PaginationMetadata)> GetProductsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<ProductDetailDto?> GetProductAsync(int productId);
        Task<ProductDto> CreateProductAsync(ProductForCreateDto product);
        Task<bool> UpdateProductAsync(int productId, ProductForUpdateDto product);
        Task<bool> DeleteProductAsync(int productId);
    }
}