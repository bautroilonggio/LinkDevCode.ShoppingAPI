using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Shopping.API.Controllers
{
    [Route("api/users/u/orders")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _orderService = orderService ??
                throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync(
            string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (orders, paginationMetadata) = await _orderService
                .GetOrdersAsync(userName, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (orders.Count() == 0)
            {
                _logger.LogInformation($"Khong co lich su mua hang cua tai khoan {userName}");

                return NotFound();
            }

            _logger.LogInformation($"Hien thi thong tin lich su mua hang cua tai khoan {userName}");

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync(OrderForCreateDto order)
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var createorderToReturn = await _orderService.CreateOrderAsync(userName, order);

                if (createorderToReturn == null)
                {
                    return NotFound();
                }

                _logger.LogInformation($"Tai khoan {userName} da tao don hang moi");

                return Created("Created new order", createorderToReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Dat hang khong thanh cong do {e.Message}");

                return BadRequest(new ResponseAPI
                {
                    Status = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrderAsync(int orderId, OrderForUpdateDto order)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _orderService.UpdateOrderAsync(userName, orderId, order))
            {
                _logger.LogWarning($"Khong tim thay don hang co id {orderId} cua tai khoan {userName}");

                return NotFound();
            }

            _logger.LogInformation($"Thong tin don hang co id {orderId} cua tai khoan {userName} da duoc cap nhat");

            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrderAsync(int orderId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _orderService.DeleteOrderAsync(userName, orderId))
            {
                _logger.LogWarning($"Khong tim thay don hang co id {orderId} cua tai khoan {userName}");

                return NotFound();
            }

            _logger.LogInformation(
                $"Thong tin don hang co id {orderId} cua tai khoan {userName} da bi xoa boi ADMIN {userName}");

            return NoContent();
        }
    }
}
