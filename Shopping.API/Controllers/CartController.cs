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
    [Route("api/users/u/carts")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService ??
                throw new ArgumentNullException(nameof(cartService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCartsAsync(
            string? searchQuery, int pageNumber = 1, int pageSize = 5)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (carts, paginationMetadata) = await _cartService
                .GetCartsAsync(userName, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (carts.Count() == 0)
            {
                return NotFound();
            }

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
                    return NotFound();
                }

                return Created("Created new cart", createCartToReturn);
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

        [HttpPut("{cartId}")]
        public async Task<ActionResult> UpdateCartAsync(int cartId, CartForUpdateDto cart)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _cartService.UpdateCartAsync(userName, cartId, cart))
            {
                return NotFound();
            }

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

            return NoContent();
        }
    }
}
