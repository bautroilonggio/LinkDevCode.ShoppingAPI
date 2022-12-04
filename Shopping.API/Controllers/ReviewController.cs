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
    /// <summary>
    /// Controller provides actions for review management
    /// </summary>
    [Route("api/v{version:apiVersion}/products/{productId}/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        /// <summary>
        /// Contructor of review controller
        /// </summary>
        /// <param name="reviewService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService ??
                throw new ArgumentNullException(nameof(reviewService));
        }

        /// <summary>
        /// Get reivews to product
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Create vew review to product
        /// </summary>
        /// <param name="productId">The id of product that will be rated</param>
        /// <param name="review">The information of review</param>
        /// <returns>ActionResult</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update information of review to product
        /// </summary>
        /// <param name="productId">The id of product that is rated</param>
        /// <param name="reviewId">The id of review that will be updated</param>
        /// <param name="review">The information of review</param>
        /// <returns>ActionResult</returns>
        [Authorize]
        [HttpPut("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete a review to product
        /// </summary>
        /// <param name="productId">The id of product</param>
        /// <param name="reviewId">The id of review</param>
        /// <returns>ActionResult</returns>
        [Authorize]
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
