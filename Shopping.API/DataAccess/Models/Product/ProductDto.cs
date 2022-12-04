using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for product without list of reviews
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Id of product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of product
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Selling price of product
        /// </summary>
        public double SellingPrice { get; set; }

        /// <summary>
        /// total quantity of product
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Creation time of product
        /// </summary>
        public DateOnly CreateAt { get; set; }

        /// <summary>
        /// Number of reviews to product
        /// </summary>
        public int NumberOfReview { get; set; }
    }
}
