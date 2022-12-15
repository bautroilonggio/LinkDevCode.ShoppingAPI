using Rating.Commons;
using Rating.DataAccess.Models;

namespace Rating.BusinessLogic.Services
{
    public interface IReviewService
    {
        Task<ReviewDto?> CreateReviewAsync(int userId,
            int orderId, int productId, ReviewForCreateDto review);
        Task<bool> DeleteReviewAsync(int userId, int productId, int reviewId);
        Task<(IEnumerable<ReviewDto>, PaginationMetadata)> GetReviewsAsync(
           int productId, int? rating, int pageNumber, int pageSize);
        Task<bool> UpdateReviewAsync(int userId,
            int productId, int reviewId, ReviewForUpdateDto review);
    }
}