using AutoMapper;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models.Product;
using Shopping.API.DataAccess.Models;
using Shopping.API.DataAccess;

namespace Shopping.API.BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int _maxReviewsPageSize = 20;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<ReviewDto>, PaginationMetadata)> GetReviewsAsync(
            int? rating, int pageNumber, int pageSize)
        {
            if (pageSize > _maxReviewsPageSize)
            {
                pageSize = _maxReviewsPageSize;
            }

            var (reviewEntities, paginationMetadata) = await _unitOfWork.ReviewRepository
                                        .GetAllAsync(rating, pageNumber, pageSize);

            var reviews = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);

            return (reviews, paginationMetadata);
        }

        public async Task<ReviewDto?> CreateReviewAsync(string userName, int productId, ReviewForCreateDto review)
        {
            var userEntity = await _unitOfWork.AccountRepository
                                   .GetUserIncludeAllAsync(u => u.UserName == userName);

            if(userEntity == null)
            {
                return null;
            }

            if(!userEntity.Orders.Any(
                o => o.OrderDetails.Any(od => od.ProductId == productId)
                    && o.Status == "Da nhan hang"))
            {
                throw new Exception("Ban can mua san pham thi moi duoc viet danh gia!");
            }

            var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(productId);

            if (productEntity == null)
            {
                return null;
            }

            if(await _unitOfWork.ReviewRepository.IsExistAsync(
                r => r.UserId == userEntity.Id && r.ProductId == productEntity.Id))
            {
                throw new Exception($"Ban da tung danh gia san pham nay!");
            }

            var reviewEntity = _mapper.Map<Review>(review);

            _unitOfWork.ReviewRepository.Add(userEntity, productEntity, reviewEntity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ReviewDto>(reviewEntity);
        }

        public async Task<bool> UpdateReviewAsync(string userName, 
            int productId, int reviewId, ReviewForUpdateDto review)
        {
            var userEntity = await _unitOfWork.AccountRepository
                                   .GetUserIncludeReviewsAsync(u => u.UserName == userName);

            if (userEntity == null)
            {
                return false;
            }

            var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(productId);

            if (productEntity == null)
            {
                return false;
            }

            var reviewEntity = await _unitOfWork.ReviewRepository.GetSingleAsync(reviewId);

            if (reviewEntity == null)
            {
                return false;
            }

            _mapper.Map(review, reviewEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(string userName, int productId, int reviewId)
        {
            var userEntity = await _unitOfWork.AccountRepository
                                   .GetUserIncludeReviewsAsync(u => u.UserName == userName);

            if (userEntity == null)
            {
                return false;
            }

            var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(productId);

            if (productEntity == null)
            {
                return false;
            }

            var reviewEntity = await _unitOfWork.ReviewRepository.GetSingleAsync(reviewId);

            if (reviewEntity == null)
            {
                return false;
            }

            _unitOfWork.ReviewRepository.Delete(userEntity, productEntity, reviewEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
