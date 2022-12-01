using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface ICartService
    {
        Task<CartDto?> CreateCartAsync(string userName, CartForCreateDto cart);
        Task<bool> DeleteCartAsync(string userName, int cartId);
        Task<CartDto?> GetCartAsync(int cartId);
        Task<(IEnumerable<CartDto>, PaginationMetadata)> GetCartsAsync(
            string userName, string? searchQuery, int pageNumber, int pageSize);
        Task<bool> UpdateCartAsync(string userName, int cartId, CartForUpdateDto cart);
    }
}