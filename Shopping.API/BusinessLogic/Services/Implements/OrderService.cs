using AutoMapper;
using Shopping.API.Commons;
using Shopping.API.DataAccess;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int _maxOrdersPageSize = 15;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<OrderDto>, PaginationMetadata)> GetOrdersAsync(
            string userName, string? searchQuery, int pageNumber, int pageSize)
        {
            var user = await _unitOfWork.UserRepository.GetSingleConditionsAsync(u => u.UserName == userName);

            if (pageSize > _maxOrdersPageSize)
            {
                pageSize = _maxOrdersPageSize;
            }

            var (orderEntities, paginationMetadata) = await _unitOfWork.OrderRepository
                                        .GetAllAsync(user.Id, searchQuery, pageNumber, pageSize);

            var orders = _mapper.Map<IEnumerable<OrderDto>>(orderEntities);

            return (orders, paginationMetadata);
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            var entity = await _unitOfWork.OrderRepository.GetSingleAsync(orderId);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<OrderDto?> CreateOrderAsync(string userName, OrderForCreateDto order)
        {
            var user = await _unitOfWork.UserRepository.GetUserIncludeCartsAsync(
                        u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            foreach(var cart in order.Carts)
            {
                // Tìm cart
                var cartEntity = await _unitOfWork.CartRepository.GetSingleAsync(cart.Id);

                // Chuyển thông tin sang OrderDetail
                var orderDetail = _mapper.Map<OrderDetail>(cartEntity);
                order.OrderDetails.Add(orderDetail);

                // Tính tổng giá tiền đơn hàng
                order.SumOfPrice += orderDetail.Price;

                // Xóa trong giỏ hàng
                _unitOfWork.CartRepository.Delete(cartEntity);

                // Cập nhật tổng số lượng sản phẩm còn lại
                var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(cartEntity.ProductId);

                if (productEntity.TotalQuantity < cartEntity.Quantity)
                {
                    throw new Exception($"So luong san pham khong du cung cap! Chi co the order toi da {productEntity.TotalQuantity} san pham");
                }

                _unitOfWork.ProductRepository.UpdateTotalQuantityProduct(productEntity, cartEntity.Quantity);
            }

            var entity = _mapper.Map<Order>(order);

            _unitOfWork.OrderRepository.Add(user, entity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<bool> UpdateOrderAsync(string userName, int orderId, OrderForUpdateDto order)
        {
            var user = await _unitOfWork.UserRepository.GetUserIncludeCartsAsync(
                        u => u.UserName == userName);

            if (user == null)
            {
                return false;
            }

            var orderEntity = await _unitOfWork.OrderRepository.GetSingleAsync(orderId);

            if (orderEntity == null)
            {
                return false;
            }

            _mapper.Map(order, orderEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteOrderAsync(string userName, int orderId)
        {
            var userEntity = await _unitOfWork.UserRepository
                                   .GetUserIncludeCartsAsync(u => u.UserName == userName);

            if (userEntity == null)
            {
                return false;
            }

            var orderEntity = await _unitOfWork.OrderRepository.GetSingleAsync(orderId);

            if (orderEntity == null)
            {
                return false;
            }

            _unitOfWork.OrderRepository.Delete(userEntity, orderEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
