using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.API.DataAccess.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public double? Price { get; set; }

        public int TotalQuantity { get; set; }

        [Required]
        public DateOnly CreateAt { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Product(string name, string brand, string category)
        {
            Name = name;
            Brand = brand;
            Category = category;
            CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString());
        }
    }
}
