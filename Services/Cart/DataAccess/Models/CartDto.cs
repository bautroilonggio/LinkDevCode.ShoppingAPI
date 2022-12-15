using Cart.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cart.DataAccess.Models
{
    /// <summary>
    /// A DTO for an cart of an account
    /// </summary>
    public class CartDto
    {
        /// <summary>
        /// Id of the cart
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of product in the cart
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Quantity of product in the cart
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price to buy the product
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Name of account buying product
        /// </summary>
        public int UserId { get; set; }
    }
}
