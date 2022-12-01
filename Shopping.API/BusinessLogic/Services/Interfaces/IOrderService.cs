using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> CreateOrderAsync(string userName, OrderForCreateDto order);
        Task<OrderDto?> GetOrderAsync(int orderId);
        Task<(IEnumerable<OrderDto>, PaginationMetadata)> GetOrdersAsync(string userName, string? searchQuery, int pageNumber, int pageSize);
        Task<bool> UpdateOrderAsync(string userName, int orderId, OrderForUpdateDto order);
        Task<bool> DeleteOrderAsync(string userName, int orderId);

    }
}