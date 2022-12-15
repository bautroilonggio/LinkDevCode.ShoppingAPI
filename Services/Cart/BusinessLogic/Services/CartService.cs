using AutoMapper;
using Cart.Commons;
using Cart.DataAccess;
using Cart.DataAccess.Entities;
using Cart.DataAccess.Models;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Cart.BusinessLogic.Services
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
            int userId, int pageNumber, int pageSize)
        {
            if (pageSize > _maxCartsPageSize)
            {
                pageSize = _maxCartsPageSize;
            }

            var (cartEntities, paginationMetadata) = await _unitOfWork.CartRepository
                                        .GetAllAsync(userId, pageNumber, pageSize);

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

        public async Task<CartDto?> CreateCartAsync(int userId, CartForCreateDto cart)
        {
            var myCart = await _unitOfWork.CartRepository.GetSingleConditionsAsync(
                                c => c.ProductId == cart.ProductId);

            if (myCart != null)
            {
                throw new Exception("Product already in the cart!");
            }

            HttpClient httpClient = new HttpClient();

            string url = string.Format("https://localhost:7295/api/v1/products/{0}", cart.ProductId);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            var product = await httpResponse.Content.ReadAsStringAsync();

            if(product == null)
            {
                throw new Exception($"Product with id {cart.ProductId} does not exist!");
            }

            var jsonDom = JsonSerializer.Deserialize<JsonObject>(product)!;
            var productTotalQuantity = (int)jsonDom["totalQuantity"]!;
            var productSellingPrice = (int)jsonDom["sellingPrice"]!;

            if (productTotalQuantity < cart.Quantity)
            {
                throw new Exception($"So luong san pham khong du cung cap! Chi co the them toi da {productTotalQuantity} san pham");
            }

            cart.Price = productSellingPrice * cart.Quantity;

            var cartEntity = _mapper.Map<DataAccess.Entities.Cart>(cart);

            cartEntity.UserId = userId;

            _unitOfWork.CartRepository.Add(cartEntity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<CartDto>(cartEntity);
        }

        public async Task<bool> UpdateCartAsync(int userId, int cartId, CartForUpdateDto cart)
        {
            var cartEntity = await _unitOfWork.CartRepository.GetSingleConditionsAsync(
                                    c => c.UserId == userId && c.Id == cartId);

            if (cartEntity == null)
            {
                return false;
            }

            HttpClient httpClient = new HttpClient();

            string url = string.Format($"https://localhost:7295/api/v1/products/{cartEntity.ProductId}");

            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            var product = await httpResponse.Content.ReadAsStringAsync();

            if(product == null)
            {
                throw new Exception($"Product with id {cartEntity.ProductId} does not exist!");
            }

            var jsonDom = JsonSerializer.Deserialize<JsonObject>(product)!;
            var productTotalQuantity = (int)jsonDom["totalQuantity"]!;
            var productSellingPrice = (int)jsonDom["sellingPrice"]!;

            if (productTotalQuantity < cart.Quantity)
            {
                throw new Exception($"So luong san pham khong du cung cap! Chi co the them toi da {productTotalQuantity} san pham");
            }

            cart.Price = productSellingPrice * cart.Quantity;

            _mapper.Map(cart, cartEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteCartAsync(int userId, int cartId)
        {
            var cartEntity = await _unitOfWork.CartRepository.GetSingleConditionsAsync(
                                    c => c.UserId == userId && c.Id == cartId);

            if (cartEntity == null)
            {
                return false;
            }

            _unitOfWork.CartRepository.Delete(cartEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
