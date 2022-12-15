using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.DataAccess.Entities
{
    public class OrderDetail
    {
        public Order? Order { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
