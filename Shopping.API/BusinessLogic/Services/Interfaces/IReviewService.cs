using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IReviewService
    {
        Task<ReviewDto?> CreateReviewAsync(string userName, int productId, ReviewForCreateDto review);
        Task<bool> DeleteReviewAsync(string userName, int productId, int reviewId);
        Task<(IEnumerable<ReviewDto>, PaginationMetadata)> GetReviewsAsync(int? rating, int pageNumber, int pageSize);
        Task<bool> UpdateReviewAsync(string userName,
            int productId, int reviewId, ReviewForUpdateDto review);
    }
}