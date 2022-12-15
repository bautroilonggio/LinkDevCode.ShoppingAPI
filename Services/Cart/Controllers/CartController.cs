using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cart.BusinessLogic.Services;
using Cart.Commons;
using Cart.DataAccess.Models;
using System.Security.Claims;
using System.Text.Json;
using Azure;

namespace Cart.Controllers
{
    /// <summary>
    /// Controller provides actions for cart management
    /// </summary>
    [Route("api/v{version:apiVersion}/users/u/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        private readonly ICartService _cartService;

        private readonly IIdentityService _identityService;

        /// <summary>
        /// Contructor of cart controller
        /// </summary>
        /// <param name="logger">Log actions</param>
        /// <param name="cartService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public CartController(ILogger<CartController> logger, ICartService cartService,
            IIdentityService identityService)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _cartService = cartService ??
                throw new ArgumentNullException(nameof(cartService));
            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));
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
            int pageNumber = 1, int pageSize = 5)
        {
            int userId = int.Parse(_identityService.GetUserIdentity());

            var (carts, paginationMetadata) = await _cartService
                .GetCartsAsync(userId, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (carts.Count() == 0)
            {
                _logger.LogInformation($"Gio hang dang trong");

                return NotFound();
            }

            _logger.LogInformation($"Hien thi gio hang");

            return Ok(carts);
        }

        /// <summary>
        /// Get card by id
        /// </summary>
        /// <param name="cartId">The id of cart</param>
        /// <returns></returns>
        [HttpGet("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDto>> GetCartAsync(int cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);

            if(cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
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
                int userId = int.Parse(_identityService.GetUserIdentity());

                var createCartToReturn = await _cartService.CreateCartAsync(userId, cart);

                if (createCartToReturn == null)
                {
                    _logger.LogInformation(
                        $"Them khong thanh cong san pham co id {cart.ProductId} vao gio hang cua tai khoan co id {userId}");

                    return NotFound();
                }

                _logger.LogInformation(
                        $"Them thanh cong san pham co id {cart.ProductId} vao gio hang cua tai khoan co id {userId}");

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
            int userId = int.Parse(_identityService.GetUserIdentity());

            if (!await _cartService.UpdateCartAsync(userId, cartId, cart))
            {
                return NotFound();
            }

            _logger.LogInformation($"Da cap nhat gio hang cua tai khoan co id {userId}");

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
            int userId = int.Parse(_identityService.GetUserIdentity());

            if (!await _cartService.DeleteCartAsync(userId, cartId))
            {
                return NotFound();
            }

            _logger.LogInformation($"Da xoa san pham trong gio hang cua tai khoan co id {userId}");

            return NoContent();
        }
    }
}
