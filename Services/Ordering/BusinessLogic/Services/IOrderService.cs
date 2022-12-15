using Ordering.Commons;
using Ordering.DataAccess.Models;

namespace Ordering.BusinessLogic.Services
{
    public interface IOrderService
    {
        Task<OrderIncludeProductsDto?> CreateOrderAsync(int userId, OrderForCreateDto order);
        Task<OrderIncludeProductsDto?> GetOrderAsync(int orderId);
        Task<(IEnumerable<OrderWithoutProductsDto>, PaginationMetadata)> GetOrdersAsync(int userId, string? searchQuery, int pageNumber, int pageSize);
        Task<bool> UpdateOrderAsync(int orderId, OrderForUpdateDto order);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<IEnumerable<OrderDetailDto>?> GetOrderDetailsAsync(int orderId);
    }
}