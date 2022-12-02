using AutoMapper;
using Shopping.API.Commons;
using Shopping.API.DataAccess;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models.Product;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int _maxCartsPageSize = 10;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<CartDto>, PaginationMetadata)> GetCartsAsync(
            string userName, string? searchQuery, int pageNumber, int pageSize)
        {
            var user = await _unitOfWork.AccountRepository.GetSingleConditionsAsync(u => u.UserName == userName);

            if (pageSize > _maxCartsPageSize)
            {
                pageSize = _maxCartsPageSize;
            }

            var (cartEntities, paginationMetadata) = await _unitOfWork.CartRepository
                                        .GetAllAsync(user.Id, searchQuery, pageNumber, pageSize);

            var carts = _mapper.Map<IEnumerable<CartDto>>(cartEntities);

            return (carts, paginationMetadata);
        }

        public async Task<CartDto?> GetCartAsync(int cartId)
        {
            var cartEntity = await _unitOfWork.CartRepository.GetSingleAsync(cartId);

            if (cartEntity == null)
            {
                return null;
            }

            return _mapper.Map<CartDto>(cartEntity);
        }

        public async Task<CartDto?> CreateCartAsync(string userName, CartForCreateDto cart)
        {
            var user = await _unitOfWork.AccountRepository.GetUserIncludeCartsAsync(
                        u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var product = await _unitOfWork.ProductRepository.GetProductIncludeCartsAsync(cart.ProductId);

            if (product == null)
            {
                return null;
            }

            if (product.TotalQuantity < cart.Quantity)
            {
                throw new Exception($"So luong san pham khong du cung cap! Chi co the them toi da {product.TotalQuantity} san pham");
            }

            if (product.SellingPrice.HasValue)
            {
                cart.Price = cart.Quantity * product.SellingPrice.Value;
            }

            var cartEntity = _mapper.Map<Cart>(cart);

            _unitOfWork.CartRepository.Add(user, product, cartEntity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<CartDto>(cartEntity);
        }

        public async Task<bool> UpdateCartAsync(string userName, int cartId, CartForUpdateDto cart)
        {
            var user = await _unitOfWork.AccountRepository.GetUserIncludeCartsAsync(
                        u => u.UserName == userName);

            if (user == null)
            {
                return false;
            }

            var cartEntity = await _unitOfWork.CartRepository.GetSingleAsync(cartId);

            if (cartEntity == null)
            {
                return false;
            }

            var product = await _unitOfWork.ProductRepository.GetProductIncludeCartsAsync(cartEntity.ProductId);

            if (product == null)
            {
                return false;
            }

            if (product.SellingPrice.HasValue)
            {
                cart.Price = cart.Quantity * product.SellingPrice.Value;
            }

            _mapper.Map(cart, cartEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteCartAsync(string userName, int cartId)
        {
            var userEntity = await _unitOfWork.AccountRepository
                                   .GetUserIncludeCartsAsync(u => u.UserName == userName);

            if (userEntity == null)
            {
                return false;
            }

            var cartEntity = await _unitOfWork.CartRepository.GetSingleAsync(cartId);

            if (cartEntity == null)
            {
                return false;
            }

            var productEntity = await _unitOfWork.ProductRepository.GetProductIncludeCartsAsync(cartEntity.ProductId);

            if (productEntity == null)
            {
                return false;
            }

            _unitOfWork.CartRepository.Delete(userEntity, productEntity, cartEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
