using Shopping.API.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.API.DataAccess.Models
{
    /// <summary>
    /// A DTO for order include orders detail
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Id of order
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Order date of order
        /// </summary>
        public DateOnly OrderDate { get; set; }

        /// <summary>
        /// Name of shipper of order
        /// </summary>
        public string? ShipName { get; set; }

        /// <summary>
        /// Address of shipper of order
        /// </summary>
        public string? ShipAddress { get; set; }

        /// <summary>
        /// Phone number of shipper of order
        /// </summary>
        public string? ShipPhone { get; set; }

        /// <summary>
        /// Status of order
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Total payment of order
        /// </summary>
        public double TotalPayment { get; set; }

        /// <summary>
        /// Number of products types in the order
        /// </summary>
        public int NumberOfOrderDetails 
        {
            get
            {
                return OrderDetails.Count;
            }
        }

        /// <summary>
        /// Information of products types in the order
        /// </summary>
        public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}
