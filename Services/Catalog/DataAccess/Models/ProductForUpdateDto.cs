using System.ComponentModel.DataAnnotations;

namespace Catalog.DataAccess.Models
{
    /// <summary>
    /// A DTO for product to update
    /// </summary>
    public class ProductForUpdateDto
    {
        /// <summary>
        /// Name of product
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// Brand of product
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Brand { get; set; }

        /// <summary>
        /// Category of product
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        /// <summary>
        /// Description of product
        /// </summary>
        [MaxLength(255)]
        public string? Description { get; set; }

        /// <summary>
        /// Selling price of product
        /// </summary>
        public double SellingPrice { get; set; }

        /// <summary>
        /// Total quantity of product
        /// </summary>
        public int TotalQuantity { get; set; }
    }
}
