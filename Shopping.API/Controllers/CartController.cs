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
    /// <summary>
    /// Controller provides actions for cart management
    /// </summary>
    [Route("api/v{version:apiVersion}/users/u/carts")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        private readonly ICartService _cartService;

        /// <summary>
        /// Contructor of cart controller
        /// </summary>
        /// <param name="logger">Log actions</param>
        /// <param name="cartService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public CartController(ILogger<CartController> logger, ICartService cartService)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _cartService = cartService ??
                throw new ArgumentNullException(nameof(cartService));
        }

        /// <summary>
        /// Get carts of account
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>ActionResult></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Add product to user's carts
        /// </summary>
        /// <param name="cart">Information cart</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update user's cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cart"></param>
        /// <returns>ActionResult</returns>
        [HttpPut("{cartId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete product in user's carts
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
