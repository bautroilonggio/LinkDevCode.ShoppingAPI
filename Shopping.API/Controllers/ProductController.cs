using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.DataAccess.Models;
using Shopping.API.DataAccess.Models.Product;
using System.Text.Json;

namespace Shopping.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ??
                throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var (products, paginationMetadata) = await _productService
                .GetProductsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            if (products.Count() == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{productId}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDetailDto>> GetProductAsync(int productId)
        {
            var product = await _productService.GetProductAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(ProductForCreateDto product)
        {
            var createProductToReturn = await _productService.CreateProductAsync(product);

            if (createProductToReturn == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute(
                    "GetProductById",
                    new
                    {
                        productId = createProductToReturn.Id
                    },
                    createProductToReturn);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProductAsync(int productId, ProductForUpdateDto product)
        {
            if (!await _productService.UpdateProductAsync(productId, product))
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProductAsync(int productId)
        {
            if (!await _productService.DeleteProductAsync(productId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
