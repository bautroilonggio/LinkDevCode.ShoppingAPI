namespace Ordering.DataAccess.Models
{
    /// <summary>
    /// A DTO for order without list of products
    /// </summary>
    public class OrderWithoutProductsDto
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
    }
}
