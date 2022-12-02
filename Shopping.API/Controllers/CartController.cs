using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Shopping.API.Controllers
{
    [Route("api/users/u/carts")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        private readonly ICartService _cartService;

        public CartController(ILogger<CartController> logger, ICartService cartService)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _cartService = cartService ??
                throw new ArgumentNullException(nameof(cartService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCartsAsync(
            string? searchQuery, int pageNumber = 1, int pageSize = 5)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (carts, paginationMetadata) = await _cartService
                .GetCartsAsync(userName, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (carts.Count() == 0)
            {
                _logger.LogInformation($"Gio hang cua tai khoan {userName} dang trong");

                return NotFound();
            }

            _logger.LogInformation($"Hien thi gio hang cua tai khoan {userName}");

            return Ok(carts);
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCartAsync(CartForCreateDto cart)
        {
            try
            {
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var createCartToReturn = await _cartService.CreateCartAsync(userName, cart);

                if (createCartToReturn == null)
                {
                    _logger.LogInformation(
                        $"Them khong thanh cong san pham co id {cart.ProductId} vao gio hang cua tai khoan {userName}");

                    return NotFound();
                }

                _logger.LogInformation(
                        $"Them thanh cong san pham co id {cart.ProductId} vao gio hang cua tai khoan {userName}");

                return Created("Created new cart", createCartToReturn);
            }
            catch (Exception e)
            {
                _logger.LogWarning(
                        $"Them san pham vao gio hang khong thanh cong do {e.Message}");

                return BadRequest(new ResponseAPI
                {
                    Status = false,
                    Message = e.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{cartId}")]
        public async Task<ActionResult> UpdateCartAsync(int cartId, CartForUpdateDto cart)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _cartService.UpdateCartAsync(userName, cartId, cart))
            {
                return NotFound();
            }

            _logger.LogInformation($"Da cap nhat gio hang cua tai khoan {userName}");

            return NoContent();
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCartAsync(int cartId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _cartService.DeleteCartAsync(userName, cartId))
            {
                return NotFound();
            }

            _logger.LogInformation($"Da xoa mot san pham trong gio hang cua tai khoan {userName}");

            return NoContent();
        }
    }
}
