using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.API.DataAccess.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        [ForeignKey("UserId")]
        public Account? User { get; set; }

        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ShipName { get; set; }

        [Required]
        [MaxLength(150)]
        public string ShipAddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShipPhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string Status { get; set; }

        [Required]
        public double TotalPayment { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Order(string shipName, string shipAddress, string shipPhone)
        {
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipPhone = shipPhone;
            Status = "Dang chuan bi hang";
        }
    }
}
