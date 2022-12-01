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
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
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
                return NotFound();
            }

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

                return Created("Created new order", createorderToReturn);
            }
            catch (Exception e)
            {
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
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrderAsync(int orderId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _orderService.DeleteOrderAsync(userName, orderId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
