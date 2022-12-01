using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.DataAccess.Models;
using System.Text.Json;
using Shopping.API.Commons;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Shopping.API.Controllers
{
    [Route("api/products/{productId}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService ??
                throw new ArgumentNullException(nameof(reviewService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetReviewsAsync(
            int? rating, int pageNumber = 1, int pageSize = 10)
        {
            var (reviews, paginationMetadata) = await _reviewService
                .GetReviewsAsync(rating, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (reviews.Count() == 0)
            {
                return NotFound();
            }

            return Ok(reviews);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReviewAsync(int productId, ReviewForCreateDto review)
        {
            try
            {
                // Lấy userName từ người dùng hiện tại
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var createReviewToReturn = await _reviewService
                                                .CreateReviewAsync(userName, productId, review);

                if (createReviewToReturn == null)
                {
                    return NotFound();
                }

                return Created("Created new review", createReviewToReturn);
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

        [Authorize]
        [HttpPut("{reviewId}")]
        public async Task<ActionResult> UpdateReviewAsync(
            int productId, int reviewId, ReviewForUpdateDto review)
        {
            // Lấy userName từ người dùng hiện tại
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _reviewService.UpdateReviewAsync(userName, productId, reviewId, review))
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{reviewId}")]
        public async Task<ActionResult> DeleteReviewAsync(int productId, int reviewId)
        {
            // Lấy userName từ người dùng hiện tại
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _reviewService.DeleteReviewAsync(userName, productId, reviewId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
