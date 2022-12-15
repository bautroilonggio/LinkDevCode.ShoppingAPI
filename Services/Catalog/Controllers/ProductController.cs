using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalog.BusinessLogic.Services;
using Catalog.DataAccess.Models;
using Catalog.DataAccess.Models.Product;
using System.Text.Json;

namespace Catalog.Controllers
{
    /// <summary>
    /// Controller provides actions for product management
    /// </summary>
    [Route("api/v{version:apiVersion}/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Contructor of product controller
        /// </summary>
        /// <param name="productService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public ProductController(IProductService productService)
        {
            _productService = productService ??
                throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="name">Product name you want to search</param>
        /// <param name="searchQuery">Information of product you want to search</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Get products by id
        /// </summary>
        /// <param name="productId">The id of product</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{productId}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int productId)
        {
            var product = await _productService.GetProductAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a new order by ADMIN account
        /// </summary>
        /// <param name="product">The information of new product</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update information product by ADMIN account
        /// </summary>
        /// <param name="productId">The id of product that will be updated</param>
        /// <param name="product">The information update of product</param>
        /// <returns>ActionResult</returns>
        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProductAsync(int productId, ProductForUpdateDto product)
        {
            if (!await _productService.UpdateProductAsync(productId, product))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete product by ADMIN account
        /// </summary>
        /// <param name="productId">The Id of product that will be deleted</param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
