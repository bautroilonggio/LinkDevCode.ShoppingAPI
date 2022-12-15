using Ordering.DataAccess.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ordering.DataAccess.Models
{
    /// <summary>
    /// A DTO for order to create new order
    /// </summary>
    public class OrderForCreateDto
    {
        /// <summary>
        /// Name of shipper of the order
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? ShipName { get; set; }

        /// <summary>
        /// Address of shipper of the order
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string? ShipAddress { get; set; }

        /// <summary>
        /// Phone number of shipper of the order
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string? ShipPhone { get; set; }

        /// <summary>
        /// Total payment of the order
        /// </summary>
        [JsonIgnore]
        public double TotalPayment { get; set; }

        /// <summary>
        /// Carts in the order
        /// </summary>
        public ICollection<CartForCreateOrderDto> Carts { get; set; } = new List<CartForCreateOrderDto>();

        /// <summary>
        /// Order Details in the order
        /// </summary>
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
