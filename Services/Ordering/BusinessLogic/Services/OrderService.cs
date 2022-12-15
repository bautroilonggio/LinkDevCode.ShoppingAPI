using AutoMapper;
using Ordering.Commons;
using Ordering.DataAccess;
using Ordering.DataAccess.Entities;
using Ordering.DataAccess.Models;
using System.Text.Json.Nodes;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Ordering.BusinessLogic.Services
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

        public async Task<(IEnumerable<OrderWithoutProductsDto>, PaginationMetadata)> GetOrdersAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize)
        {
            if (pageSize > _maxOrdersPageSize)
            {
                pageSize = _maxOrdersPageSize;
            }

            var (orderEntities, paginationMetadata) = await _unitOfWork.OrderRepository
                                        .GetAllAsync(userId, searchQuery, pageNumber, pageSize);

            var orders = _mapper.Map<IEnumerable<OrderWithoutProductsDto>>(orderEntities);

            return (orders, paginationMetadata);
        }

        public async Task<OrderIncludeProductsDto?> GetOrderAsync(int orderId)
        {
            var entity = await _unitOfWork.OrderRepository.GetOrderAsync(o => o.Id == orderId);

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<OrderIncludeProductsDto>(entity);
        }

        public async Task<IEnumerable<OrderDetailDto>?> GetOrderDetailsAsync(int orderId)
        {
            var entities = await _unitOfWork.OrderDetailRepository.GetManyAsync(
                                o => o.OrderId == orderId);

            if(entities == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<OrderDetailDto>>(entities);
        }

        public async Task<OrderIncludeProductsDto?> CreateOrderAsync(int userId, OrderForCreateDto order)
        {
            HttpClient httpClient = new HttpClient();

            foreach (var cart in order.Carts)
            {
                // Tìm cart
                string urlCart = string.Format($"https://localhost:7078/api/v1/users/u/carts/{cart.Id}");

                HttpResponseMessage httpResponseGetCart = await httpClient.GetAsync(urlCart);

                if (!httpResponseGetCart.IsSuccessStatusCode)
                {
                    throw new Exception($"Cart with id {cart.Id} does not exist!");
                }

                var myCart = await httpResponseGetCart.Content.ReadAsStringAsync();

                var jsonDomCart = JsonSerializer.Deserialize<JsonObject>(myCart)!;

                // Kiểm tra số lượng sản phẩm còn lại trong cửa hàng
                var productId = (int)jsonDomCart["productId"]!;

                var productQuantityOfCart = (int)jsonDomCart["quantity"]!;

                string urlProduct = string.Format($"https://localhost:7072/api/v1/products/{productId}");

                HttpResponseMessage httpResponseGetProduct = await httpClient.GetAsync(urlProduct);

                if (!httpResponseGetProduct.IsSuccessStatusCode)
                {
                    throw new Exception($"Product with id {productId} does not exist!");
                }

                var myProduct = await httpResponseGetProduct.Content.ReadAsStringAsync();

                var jsonDomProduct = JsonSerializer.Deserialize<JsonObject>(myProduct)!;

                var product = new ProductForUpdateDto();
                product.Name = (string)jsonDomProduct["name"]!;
                product.Brand = (string)jsonDomProduct["brand"]!;
                product.Category = (string)jsonDomProduct["category"]!;
                product.Description = (string)jsonDomProduct["description"]!;
                product.SellingPrice = (int)jsonDomProduct["sellingPrice"]!;
                product.TotalQuantity = (int)jsonDomProduct["totalQuantity"]!;

                if (product.TotalQuantity < productQuantityOfCart)
                {
                    throw new Exception($"So luong san pham khong du cung cap! Chi co the order toi da {product.TotalQuantity} san pham");
                }

                // Chuyển thông tin sang OrderDetail
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ProductId = (int)jsonDomCart["productId"]!;
                orderDetail.Quantity = (int)jsonDomCart["quantity"]!;
                orderDetail.Price = (int)jsonDomCart["price"]!;

                // Thêm order detail vào bảng
                order.OrderDetails.Add(orderDetail);

                // Tính tổng giá tiền đơn hàng
                order.TotalPayment += orderDetail.Price;

                // Xóa trong giỏ hàng
                await httpClient.DeleteAsync(urlCart);

                // Cập nhật số lượng sản phẩm
                product.TotalQuantity -= productQuantityOfCart;

                var productJson = JsonSerializer.Serialize(product);

                var productRequestContent = new StringContent(productJson, Encoding.UTF8, "application/json");

                await httpClient.PutAsync(urlProduct, productRequestContent);
            }

            var entity = _mapper.Map<Order>(order);

            entity.UserId = userId;

            _unitOfWork.OrderRepository.Add(entity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderIncludeProductsDto>(entity);
        }

        public async Task<bool> UpdateOrderAsync(int orderId, OrderForUpdateDto order)
        {
            var orderEntity = await _unitOfWork.OrderRepository.GetSingleAsync(orderId);

            if (orderEntity == null)
            {
                return false;
            }

            _mapper.Map(order, orderEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var orderEntity = await _unitOfWork.OrderRepository.GetSingleAsync(orderId);

            if (orderEntity == null)
            {
                return false;
            }

            _unitOfWork.OrderRepository.Delete(orderEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
