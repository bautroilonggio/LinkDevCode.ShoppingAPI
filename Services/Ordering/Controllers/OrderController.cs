using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.BusinessLogic.Services;
using Ordering.Commons;
using Ordering.DataAccess.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Ordering.Controllers
{
    /// <summary>
    /// Controller provides actions for order management
    /// </summary>
    [Route("api/v{version:apiVersion}/users/u/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _orderService;

        private readonly IIdentityService _identityService;

        /// <summary>
        /// Contructor of order controller
        /// </summary>
        /// <param name="logger">Log actions</param>
        /// <param name="orderService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public OrderController(ILogger<OrderController> logger, IOrderService orderService,
            IIdentityService identityService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _orderService = orderService ??
                throw new ArgumentNullException(nameof(orderService));
            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));
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
        public async Task<ActionResult<IEnumerable<OrderIncludeProductsDto>>> GetOrdersAsync(
            string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            int userId = int.Parse(_identityService.GetUserIdentity());

            var (orders, paginationMetadata) = await _orderService
                .GetOrdersAsync(userId, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (orders.Count() == 0)
            {
                _logger.LogInformation($"Khong co lich su mua hang cua tai khoan");

                return NotFound();
            }

            _logger.LogInformation($"Hien thi thong tin lich su mua hang cua tai khoan");

            return Ok(orders);
        }

        /// <summary>
        /// Get order include list of products by id
        /// </summary>
        /// <param name="orderId">The id of order</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderIncludeProductsDto>> GetOrderAsync(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// Get detail of order by id
        /// </summary>
        /// <param name="orderId">The id of order</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{orderId}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetailsAsync(int orderId)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(orderId);

            if (orderDetails == null)
            {
                return NotFound();
            }

            return Ok(orderDetails);
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
        public async Task<ActionResult<OrderIncludeProductsDto>> CreateOrderAsync(OrderForCreateDto order)
        {
            try
            {
                int userId = int.Parse(_identityService.GetUserIdentity());

                var createorderToReturn = await _orderService.CreateOrderAsync(userId, order);

                if (createorderToReturn == null)
                {
                    return NotFound();
                }

                _logger.LogInformation($"Tai khoan co id {userId} da tao don hang moi");

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
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrderAsync(int orderId, OrderForUpdateDto order)
        {
            if (!await _orderService.UpdateOrderAsync(orderId, order))
            {
                _logger.LogWarning($"Khong tim thay don hang co id {orderId}");

                return NotFound();
            }

            _logger.LogInformation($"Thong tin don hang co id {orderId} da duoc cap nhat");

            return NoContent();
        }

        /// <summary>
        /// Delete order by ADMIN account
        /// </summary>
        /// <param name="orderId">The id of order that will be deleted</param>
        /// <returns></returns>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrderAsync(int orderId)
        {
            if (!await _orderService.DeleteOrderAsync(orderId))
            {
                _logger.LogWarning($"Khong tim thay don hang co id {orderId}");

                return NotFound();
            }

            _logger.LogInformation(
                $"Thong tin don hang co id {orderId} da bi xoa boi ADMIN");

            return NoContent();
        }
    }
}
