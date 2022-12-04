using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shopping.API.DataAccess.Models.Product
{
    /// <summary>
    /// A DTO for product for create
    /// </summary>
    public class ProductForCreateDto
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

        /// <summary>
        /// Creation time of order
        /// </summary>
        [Required]
        [JsonIgnore]
        public DateOnly CreateAt { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());
    }
}
