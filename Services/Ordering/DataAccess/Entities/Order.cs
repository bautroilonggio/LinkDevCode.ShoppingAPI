using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.DataAccess.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; } = DateOnly.Parse(DateTime.Now.ToShortDateString());

        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ShipName { get; set; }

        [Required]
        [MaxLength(150)]
        public string? ShipAddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string? ShipPhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Status { get; set; } = "Dang chuan bi hang";

        [Required]
        public double TotalPayment { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
