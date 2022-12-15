using Cart.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cart.DataAccess.Models
{
    /// <summary>
    /// A DTO for an cart to update
    /// </summary>
    public class CartForUpdateDto
    {
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
