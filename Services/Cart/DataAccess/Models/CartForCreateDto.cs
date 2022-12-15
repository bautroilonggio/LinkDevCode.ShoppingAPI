using Cart.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cart.DataAccess.Models
{
    /// <summary>
    /// A DTO for an cart to add products
    /// </summary>
    public class CartForCreateDto
    {
        /// <summary>
        /// Id of product
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of product
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// The price to buy the product
        /// </summary>
        [JsonIgnore]
        public double Price { get; set; }
    }
}
