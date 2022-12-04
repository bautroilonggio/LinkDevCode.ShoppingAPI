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
    /// <summary>
    /// Controller provides actions for order management
    /// </summary>
    [Route("api/v{version:apiVersion}/users/u/orders")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _orderService;

        /// <summary>
        /// Contructor of order controller
        /// </summary>
        /// <param name="logger">Log actions</param>
        /// <param name="orderService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _orderService = orderService ??
                throw new ArgumentNullException(nameof(orderService));
        }

        /// <summary>
        /// Get orders of account
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>ActionResult></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="order">The information of new order</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update order by ADMIN account
        /// </summary>
        /// <param name="orderId">The id of order that will be updated</param>
        /// <param name="order">The information update of order</param>
        /// <returns>ActionResult</returns>
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete order by ADMIN account
        /// </summary>
        /// <param name="orderId">The id of order that will be deleted</param>
        /// <returns></returns>
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
