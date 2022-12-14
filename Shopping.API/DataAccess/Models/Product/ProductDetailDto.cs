using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for product include list of reviews
    /// </summary>
    public class ProductDetailDto
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
        /// Brand of product
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// Category of product
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Description of product
        /// </summary>
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
        /// Creation time of product
        /// </summary>
        public DateOnly CreateAt { get; set; }

        /// <summary>
        /// Number of review to product
        /// </summary>
        public int NumberOfReview
        {
            get
            {
                return Reviews.Count;
            }
        }

        /// <summary>
        /// List of reviews to product
        /// </summary>
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
