using System.ComponentModel.DataAnnotations;

namespace Ordering.DataAccess.Models
{
    /// <summary>
    /// A DTO for order to update
    /// </summary>
    public class OrderForUpdateDto
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
        /// Status of the order
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Status { get; set; }
    }
}
