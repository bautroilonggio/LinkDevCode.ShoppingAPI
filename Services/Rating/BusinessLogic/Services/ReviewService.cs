using AutoMapper;
using Rating.Commons;
using Rating.DataAccess.Entities;
using Rating.DataAccess.Models;
using Rating.DataAccess;
using System;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Rating.BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int _maxReviewsPageSize = 10;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<ReviewDto>, PaginationMetadata)> GetReviewsAsync(
           int productId,  int? rating, int pageNumber, int pageSize)
        {
            if (pageSize > _maxReviewsPageSize)
            {
                pageSize = _maxReviewsPageSize;
            }

            var (reviewEntities, paginationMetadata) = await _unitOfWork.ReviewRepository
                                        .GetAllAsync(productId, rating, pageNumber, pageSize);

            var reviews = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);

            return (reviews, paginationMetadata);
        }

        public async Task<ReviewDto?> CreateReviewAsync(int userId, 
            int orderId, int productId, ReviewForCreateDto review)
        {
            if (await _unitOfWork.ReviewRepository.IsExistAsync(
                r => r.UserId == userId && r.ProductId == productId))
            {
                throw new Exception($"There are already product reviews by users with id {userId}!");
            }

            // Kiểm tra trạng thái đơn hàng
            HttpClient httpClient = new HttpClient();

            string urlOrder = string.Format($"https://localhost:7299/api/v1/users/u/orders/{orderId}");

            HttpResponseMessage httpResponse = await httpClient.GetAsync(urlOrder);

            var order = await httpResponse.Content.ReadAsStringAsync();

            var jsonDomOrder = JsonSerializer.Deserialize<JsonObject>(order)!;

            var statusOrder = (string)jsonDomOrder["status"]!;

            if(statusOrder != "Da nhan hang")
            {
                throw new Exception("The user has not received the goods!");
            }

            var reviewEntity = _mapper.Map<Review>(review);

            reviewEntity.UserId = userId;
            reviewEntity.ProductId = productId;

            _unitOfWork.ReviewRepository.Add(reviewEntity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ReviewDto>(reviewEntity);
        }

        public async Task<bool> UpdateReviewAsync(int userId, 
            int productId, int reviewId, ReviewForUpdateDto review)
        {
            var reviewEntity = await _unitOfWork.ReviewRepository.GetSingleConditionsAsync(
                                     r => r.UserId == userId && r.ProductId == productId && r.Id == reviewId);

            if (reviewEntity == null)
            {
                return false;
            }

            _mapper.Map(review, reviewEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(int userId, int productId, int reviewId)
        {
            var reviewEntity = await _unitOfWork.ReviewRepository.GetSingleConditionsAsync(
                                     r => r.UserId == userId && r.ProductId == productId && r.Id == reviewId);

            if (reviewEntity == null)
            {
                return false;
            }

            _unitOfWork.ReviewRepository.Delete(reviewEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
