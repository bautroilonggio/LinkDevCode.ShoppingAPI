using AutoMapper;
using Catalog.Commons;
using Catalog.DataAccess;
using Catalog.DataAccess.Entities;
using Catalog.DataAccess.Models;
using Catalog.DataAccess.Models.Product;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.Json;
using System;

namespace Catalog.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private const int _maxProductsPageSize = 20;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<(IEnumerable<ProductDto>, PaginationMetadata)> GetProductsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            if (pageSize > _maxProductsPageSize)
            {
                pageSize = _maxProductsPageSize;
            }

            var (productEntities, paginationMetadata) = await _unitOfWork.ProductRepository
                                        .GetAllAsync(name, searchQuery, pageNumber, pageSize);

            var products = _mapper.Map<IEnumerable<ProductDto>>(productEntities);

            return (products, paginationMetadata);
        }

        public async Task<ProductDto?> GetProductAsync(int productId)
        {
            var productEntity = await _unitOfWork.ProductRepository
                                      .GetSingleAsync(productId);

            if (productEntity == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(productEntity);
        }

        public async Task<ProductDto> CreateProductAsync(ProductForCreateDto product)
        {
            var productEntity = _mapper.Map<Product>(product);

            _unitOfWork.ProductRepository.Add(productEntity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProductDto>(productEntity);
        }

        public async Task<bool> UpdateProductAsync(int productId, ProductForUpdateDto product)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(productId);
            if (productEntity == null)
            {
                return false;
            }

            _mapper.Map(product, productEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetSingleAsync(productId);
            if (productEntity == null)
            {
                return false;
            }

            _unitOfWork.ProductRepository.Delete(productEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
