using Cart.Commons;
using Cart.DataAccess.Models;

namespace Cart.BusinessLogic.Services
{
    public interface ICartService
    {
        Task<CartDto?> CreateCartAsync(int userId, CartForCreateDto cart);
        Task<bool> DeleteCartAsync(int userId, int cartId);
        Task<CartDto?> GetCartAsync(int cartId);
        Task<(IEnumerable<CartDto>, PaginationMetadata)> GetCartsAsync(
            int userId, int pageNumber, int pageSize);
        Task<bool> UpdateCartAsync(int userId, int cartId, CartForUpdateDto cart);
    }
}