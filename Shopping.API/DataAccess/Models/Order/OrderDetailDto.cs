using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for OrderDetail of an order
    /// </summary>
    public class OrderDetailDto
    {
        /// <summary>
        /// Name of product in order
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Quantity of product in order
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price to buy the product
        /// </summary>
        public double Price { get; set; }
    }
}
